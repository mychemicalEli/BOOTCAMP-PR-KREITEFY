import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { SongService } from '../services/song.service';
import { LatestSongsDto } from '../models/lastest.model';

@Component({
  selector: 'app-latest',
  templateUrl: './latest.component.html',
  styleUrls: ['./latest.component.scss']
})
export class LatestComponent implements OnChanges {
  @Input() genreId: number | undefined = undefined;
  @Output() loaded = new EventEmitter<boolean>();
  latestSongs: LatestSongsDto[] = [];
  errorMessage: string | null = null;

  constructor(private songsService: SongService) { }


  ngOnChanges(changes: SimpleChanges): void {
    if (changes['genreId']) {
      this.getLatestSongs();
    }
  }

  private getLatestSongs(): void {
    this.songsService.getLatestSongs(this.genreId).subscribe({
      next: (response: LatestSongsDto[]) => {
        this.latestSongs = response;
        this.errorMessage = null;
        console.log('Latest songs loaded');
        this.loaded.emit(true);

      },
      error: (err) => {
        console.error('Error obteniendo canciones más recientes', err);
        this.errorMessage = 'No se pudieron cargar las canciones más recientes.';
        this.loaded.emit(true);
      }
    });
  }
}
