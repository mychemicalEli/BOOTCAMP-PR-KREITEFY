import { Component } from '@angular/core';
import { LatestSongs } from '../models/latest.model';
import { SongService } from '../services/song.service';
import { ArtistDto } from '../models/artists.model';
import { AlbumDto } from '../models/album.model';
import { ActivatedRoute } from '@angular/router';
import { SongDto } from '../models/song.model';

@Component({
  selector: 'app-latest',
  templateUrl: './latest.component.html',
  styleUrl: './latest.component.scss'
})
export class LatestComponent {
  artists: ArtistDto[] = [];
  albums: AlbumDto[] = [];
  latestSongs: LatestSongs[] = [];
  errorMessage: string | null = null;

  constructor(private songsService: SongService) { }

  ngOnInit(): void {
    this.getLatestSongs();
  }

  private getLatestSongs(): void {
    this.songsService.getLatestSongs().subscribe({
      next: (response: LatestSongs[]) => {
        this.latestSongs = response;
        this.errorMessage = null;
      },
      error: (err) => {
        console.error('Error obteniendo canciones más recientes', err);
        this.errorMessage = 'No se pudieron cargar las canciones más recientes. Por favor, inténtelo de nuevo.';
      },
    });
  }

  
  public getArtists(): void {
    this.songsService.getArtist().subscribe({
      next: (artists) => {
        this.artists = artists;
      },
      error: (err) => {
        console.log("Error obteniendo artistas", err);
      }
    });
  }

  public getAlbums(): void {
    this.songsService.getAlbums().subscribe({
      next: (albums) => {
        this.albums = albums;

      },
      error: (err) => {
        console.log("Error obteniendo albums", err);
      }
    });
  }
}
