import { Component, SimpleChanges } from '@angular/core';
import { SongService } from '../services/song.service';
import { PaginatedResponse, SongListDto } from '../models/all-songs.model';
import { AuthService } from '../../auth/services/auth.service';
import { LoadingService } from '../../shared/spinner/service/loading.service';

@Component({
  selector: 'app-song-list',
  templateUrl: './song-list.component.html',
  styleUrl: './song-list.component.scss'
})
export class SongListComponent {
  isLoggedIn: boolean = false;
  data: any = null;
  loading: boolean = false;

  currentPage: number = 1;
  totalPages: number = 0;
  pageSize: number = 8;
  totalCount: number = 0;
  songs: SongListDto[] = [];
  errorMessage: string | null = null;
  noResultsFound: boolean = false;
  loadError: boolean = false;

  titleFilter?: string;
  artistFilter?: string = '';
  albumFilter?: string = '';
  genreFilter?: string = '';
  genres: { id: number; name: string }[] = [];
  albums: { id: number; title: string }[] = [];
  artists: { id: number; name: string }[] = [];

  constructor(private songsService: SongService, private authService: AuthService, private loadingService: LoadingService) { }
  
  ngOnInit(): void {
    this.loadingService.loading$.subscribe((isLoading) => {
      this.loading = isLoading;
    });
    this.authService.getLoggedInStatus().subscribe(status => {
      this.isLoggedIn = status;
      if (this.isLoggedIn) {
        this.getAllSongs();
        this.fetchGenres();
        this.fetchAlbums();
        this.fetchArtists();
      } else {
        this.data = null;
      }
    });
  }

  private getAllSongs(): void {
    const filters: string | undefined = this.buildFilters();
    this.loadingService.show();
    this.songsService.getAllSongs(this.currentPage, this.pageSize, filters).subscribe({
      next: (response: PaginatedResponse<SongListDto>) => {
        this.songs = response.data;
        this.currentPage = response.currentPage;
        this.totalPages = response.totalPages;
        this.pageSize = response.pageSize;
        this.totalCount = response.totalCount;
        this.noResultsFound = this.songs.length === 0;
        this.loadingService.hide();
      },
      error: (err) => {
        this.loadError = true;
        this.handleError(err);
        this.loadingService.hide();
      }
    });
  }

  private handleError(error: any): void {
    console.log(error);
  }

  fetchGenres(): void {
    this.songsService.getGenres().subscribe({
      next: (response) => {
        this.genres = response;
      },
      error: (err) => {
        console.error('Error al obtener géneros', err);
      }
    });
  }

  fetchAlbums(): void {
    this.songsService.getAlbums().subscribe({
      next: (response) => {
        this.albums = response;
      },
      error: (err) => {
        console.error('Error al obtener álbums', err);
      }
    });
  }

  fetchArtists(): void {
    this.songsService.getArtists().subscribe({
      next: (response) => {
        this.artists = response;
      },
      error: (err) => {
        console.error('Error al obtener álbums', err);
      }
    });
  }

  private buildFilters(): string | undefined {
    const filters: string[] = [];

    if (this.titleFilter) filters.push(`title:MATCH:${this.titleFilter}`);
    if (this.artistFilter) filters.push(`artist.name:MATCH:${this.artistFilter}`);
    if (this.albumFilter) filters.push(`album.title:MATCH:${this.albumFilter}`);
    if (this.genreFilter) filters.push(`genre.name:MATCH:${this.genreFilter}`);
    return filters.length > 0 ? filters.join(",") : undefined;
  }


  public searchByFilters(): void {
    this.currentPage = 1;
    this.getAllSongs();
  }

  public resetFilters(): void {
    this.titleFilter = '';
    this.artistFilter = '';
    this.albumFilter = '';
    this.genreFilter = '';
    this.searchByFilters();
  }

  public previousPage(): void {
    this.currentPage = this.currentPage - 1;
    this.getAllSongs();
  }
  public nextPage(): void {
    this.currentPage = this.currentPage + 1;
    this.getAllSongs();
  }
}
