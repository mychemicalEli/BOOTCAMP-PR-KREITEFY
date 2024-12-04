import { Component } from '@angular/core';
import { LatestSongs } from '../models/latest.model';
import { SongService } from '../services/song.service';


@Component({
  selector: 'app-latest',
  templateUrl: './latest.component.html',
  styleUrl: './latest.component.scss'
})
export class LatestComponent {
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
  
}
