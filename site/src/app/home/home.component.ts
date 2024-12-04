import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/services/auth.service';
import { SongService } from '../songs/services/song.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  isLoggedIn: boolean = false;
  data: any = null;
  selectedGenre: number | undefined = undefined;
  genres: { id: number; name: string }[] = [];

  constructor(private authService: AuthService, private songService: SongService) { }

  ngOnInit(): void {
    this.authService.getLoggedInStatus().subscribe(status => {
      this.isLoggedIn = status;
      if (this.isLoggedIn) {
        this.fetchUserData();
        this.fetchGenres();
      } else {
        this.data = null;
      }
    });
  }

  fetchUserData(): void {
    this.authService.getMe().subscribe(
      (userData) => {
        this.data = userData;
      },
      (error) => {
        console.error('Error al obtener los datos del usuario:', error);
        this.data = null;
      }
    );
  }

  fetchGenres(): void {
    this.songService.getGenres().subscribe({
      next: (response) => {
        this.genres = response;
      },
      error: (err) => {
        console.error('Error al obtener géneros', err);
      }
    });
  }

  onGenreChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.selectedGenre = value ? Number(value) : undefined;
  }
}
