import { Component } from '@angular/core';
import { LatestSongs } from '../models/latest.model';
import { SongService } from '../services/song.service';
import { ArtistDto } from '../models/artists.model';
import { AlbumDto } from '../models/album.model';

@Component({
  selector: 'app-latest',
  templateUrl: './latest.component.html',
  styleUrl: './latest.component.scss'
})
export class LatestComponent {
  artists: ArtistDto[] = [];
  albums: AlbumDto[] = [];
  latestSongs: LatestSongs[] = [];
  constructor(private songsService: SongService) { }

  ngOnInit(): void {
    this.getLatestSongs();
  }

  private getLatestSongs(): void {
    this.songsService.getLatestSongs().subscribe({
      next: (response: LatestSongs[]) => {
        this.latestSongs = response; // Ajuste aquí: No necesitas acceder a `.data`
      },
      error: (err) => {
        console.error('Error obteniendo canciones más recientes', err);
      },
    });
  }

  public getArtists(): void {
    this.songsService.getArtist().subscribe({
      next: (artists) => {
        this.artists = artists;
      },
      error: (err) => { console.log("Error obteniendo artistas") }
    });
  }

  public getAlbums(): void {
    this.songsService.getAlbums().subscribe({
      next: (albums) => {
        this.albums = albums;
      },
      error: (err) => { console.log("Error obteniendo albums") }
    });
  }
}


