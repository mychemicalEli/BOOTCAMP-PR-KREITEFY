import { Component, OnInit } from '@angular/core';
import { SongDto } from '../models/song.model';
import { SongService } from '../services/song.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../auth/services/auth.service';
import { RatingDto } from '../models/rating.model';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent implements OnInit {
  songId?: number;
  song?: SongDto;
  errorMessage: string | null = null;

  constructor(
    private songsService: SongService, 
    private route: ActivatedRoute,
    private authService: AuthService ) { }

  ngOnInit(): void {
    const entryParam: string | null = this.route.snapshot.paramMap.get("id");
    if (entryParam !== null) {
      this.songId = +entryParam;
      this.getSongById(this.songId);
    }
  }

  private getSongById(songId: number) {
    this.songsService.getSongById(songId).subscribe({
      next: (songRequest) => {
        this.song = songRequest;
      },
      error: (err) => {
        console.error('Error obteniendo el detalle de la canción', err);
        this.errorMessage = 'No se pudo cargar el detalle de esta canción. Por favor, inténtelo de nuevo.';
      }
    });
  }

  rateSong(star: number) {
    if (this.song) {
      const userId = localStorage.getItem('userId');
      if (!userId) {
        console.error('No se puede añadir valoración: usuario no autenticado.');
        return;
      }
  
      const ratingDto = {
        userId: +userId,
        songId: this.song.id,
        stars: star
      };
  
      this.songsService.addRating(ratingDto).subscribe({
        next: () => {
          this.getSongById(this.song!.id);
        },
        error: (err) => {
          console.error('Error al añadir valoración:', err);
        }
      });
    }
  }
  
  playSong() {
    if (this.song) {
      this.songsService.incrementStreams(this.song.id).subscribe({
        next: () => {
          this.song!.streams += 1;
        },
        error: (err) => {
          console.error('Error incrementando las reproducciones', err);
        }
      });
    }
  }  
}
