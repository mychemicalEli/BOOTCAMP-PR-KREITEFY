import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { SongService } from '../services/song.service';
import { LatestSongs } from '../models/latest.model';

@Component({
  selector: 'app-latest',
  templateUrl: './latest.component.html',
  styleUrls: ['./latest.component.scss']
})
export class LatestComponent implements OnChanges {
  @Input() genreId: number | undefined = undefined;
  latestSongs: LatestSongs[] = [];
  errorMessage: string | null = null;

  constructor(private songsService: SongService) {}


  ngOnChanges(changes: SimpleChanges): void {
    if (changes['genreId']) {
      this.getLatestSongs();
    }
  }

  private getLatestSongs(): void {
    this.songsService.getLatestSongs(this.genreId).subscribe({
      next: (response: LatestSongs[]) => {
        this.latestSongs = response;
        this.errorMessage = null;
      },
      error: (err) => {
        console.error('Error obteniendo canciones más recientes', err);
        this.errorMessage = 'No se pudieron cargar las canciones más recientes. Por favor, inténtelo de nuevo.';
      }
    });
  }
}
