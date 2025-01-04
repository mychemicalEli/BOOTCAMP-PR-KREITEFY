import { Component, OnInit } from '@angular/core';
import { SongDetailDto } from '../models/song-detail.model';
import { SongService } from '../services/song.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../auth/services/auth.service';
import { UserDtoResponse } from '../../auth/models/userdto.response';
import { LoadingService } from '../../shared/spinner/service/loading.service';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent implements OnInit {
  isLoggedIn: boolean = false;
  data: any = null;
  loading: boolean = false;

  songId?: number;
  song?: SongDetailDto;
  errorMessage: string | null = null;
  userId?: number;

  constructor(
    private songsService: SongService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private loadingService: LoadingService) { }

  ngOnInit(): void {
    this.loadingService.loading$.subscribe((isLoading) => {
      this.loading = isLoading;
    });
    this.authService.getLoggedInStatus().subscribe(status => {
      this.isLoggedIn = status;
      if (this.isLoggedIn) {
        const entryParam: string | null = this.route.snapshot.paramMap.get("id");
        if (entryParam !== null) {
          this.songId = +entryParam;
          this.getSongById(this.songId);
        }
        this.authService.getMe().subscribe({
          next: (user: UserDtoResponse) => {
            this.userId = user.id;
          },
          error: (err) => {
            console.error('Error al obtener los datos del usuario', err);
          }
        });

      } else {
        this.data = null;
      }
    });
  }

  private getSongById(songId: number) {
    this.loadingService.show();
    this.songsService.getSongById(songId).subscribe({
      next: (songRequest) => {
        this.song = songRequest;
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Error obteniendo el detalle de la canción', err);
        this.errorMessage = 'No se pudo cargar el detalle de esta canción. Por favor, inténtelo de nuevo.';
        this.loadingService.hide();
      }
    });
  }

  rateSong(star: number) {
    if (this.song && this.userId) {
      const ratingDto = {
        userId: this.userId,
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
    if (this.song && this.userId) {
      this.songsService.incrementStreams(this.song.id, this.userId).subscribe({
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
