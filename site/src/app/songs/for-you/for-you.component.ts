import { Component, EventEmitter, Output } from '@angular/core';
import { SongService } from '../services/song.service';
import { YourSongsDto } from '../models/yourSongs.model';

@Component({
  selector: 'app-for-you',
  templateUrl: './for-you.component.html',
  styleUrl: './for-you.component.scss'
})
export class ForYouComponent {
  @Output() loaded = new EventEmitter<boolean>();
  forYouSongs: YourSongsDto[] = [];
  errorMessage: string | null = null;

  constructor(private songsService: SongService) { }
  ngOnInit(): void {
    this.getForYouSongs();

  }
  private getForYouSongs(): void {
    this.songsService.getForYouSongs().subscribe({
      next: (response: YourSongsDto[]) => {
        this.forYouSongs = response;
        this.errorMessage = null;
        this.loaded.emit(true);
        console.log('For you songs loaded');
      },
      error: (err) => {
        console.error('Error obteniendo canciones para ti', err);
        this.errorMessage = 'No se pudieron cargar tus canciones.';
        this.loaded.emit(true);
      }
    });
  }
}
