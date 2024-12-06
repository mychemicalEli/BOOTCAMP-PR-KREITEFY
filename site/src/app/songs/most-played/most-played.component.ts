import { Component, Input, SimpleChanges } from '@angular/core';
import { MostPlayedSongsDto } from '../models/most-played.model';
import { SongService } from '../services/song.service';

@Component({
  selector: 'app-most-played',
  templateUrl: './most-played.component.html',
  styleUrl: './most-played.component.scss'
})
export class MostPlayedComponent {
  @Input() genreId: number | undefined = undefined;
  mostPlayedSongs: MostPlayedSongsDto[] = [];
  errorMessage: string | null = null;

  constructor(private songsService: SongService) { }


  ngOnChanges(changes: SimpleChanges): void {
    if (changes['genreId']) {
      this.getMostPlayedSongs();
    }
  }

  private getMostPlayedSongs(): void {
    this.songsService.getMostPlayedSongs(this.genreId).subscribe({
      next: (response: MostPlayedSongsDto[]) => {
        this.mostPlayedSongs = response;
        this.errorMessage = null;
      },
      error: (err) => {
        console.error('Error obteniendo canciones con más reproducciones.', err);
        this.errorMessage = 'No se pudieron cargar las canciones con más reproducciones.';
      }
    });
  }
}
