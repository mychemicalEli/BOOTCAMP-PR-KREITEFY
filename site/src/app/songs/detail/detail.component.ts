import { Component, OnInit } from '@angular/core';
import { SongDto } from '../models/song.model';
import { SongService } from '../services/song.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent implements OnInit {
  songId?: number;
  song?: SongDto;
  errorMessage: string | null = null;

  constructor(private songsService: SongService, private route: ActivatedRoute) { }

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
      this.song.mediaRating = star;
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
