import { Component } from '@angular/core';
import { SongService } from '../services/song.service';
import { PaginatedResponse, SongListDto } from '../models/all-songs.model';

@Component({
  selector: 'app-song-list',
  templateUrl: './song-list.component.html',
  styleUrl: './song-list.component.scss'
})
export class SongListComponent {
  currentPage: number = 1;
  totalPages: number = 0;
  pageSize: number = 10;
  totalCount: number = 0;
  songs: SongListDto[] = [];
  errorMessage: string | null = null;

  constructor(private songsService: SongService) {}
  ngOnInit(): void {
    this.getAllSongs();
  }

  private getAllSongs(): void {
    const filters: string | undefined = '';
    this.songsService.getAllSongs(this.currentPage, this.pageSize, filters).subscribe({
      next: (response: PaginatedResponse<SongListDto>) => {
        this.songs = response.data;
        this.currentPage = response.currentPage;
        this.totalPages = response.totalPages;
        this.pageSize = response.pageSize;
        this.totalCount = response.totalCount;
      },
      error: (err) => { 
        console.error('Error obteniendo canciones más recientes', err);
        this.errorMessage = 'No se pudieron cargar las canciones más recientes.'; }
    });
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
