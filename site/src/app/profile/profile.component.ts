import { Component, OnInit } from '@angular/core';
import { ProfileService } from './service/profile.service';
import { AuthService } from '../auth/services/auth.service';
import { HistorySongsDto, PaginatedResponse } from './models/history.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  userId: number | null = null;
  history: HistorySongsDto[] = [];
  currentPage: number = 1;
  totalPages: number = 0;
  pageSize: number = 8;
  totalCount: number = 0;

  constructor(
    private profileService: ProfileService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService.getMe().subscribe({
      next: (user) => {
        this.userId = user.id;
        this.getHistory();
      },
      error: (err) => {
        console.log("Error al obtener el usuario loggeado")
      }
    });
  }

   getHistory(): void {
    this.profileService.getHistorySongs(this.currentPage, this.pageSize).subscribe({
      next: (response: PaginatedResponse<HistorySongsDto>) => {
        this.history = response.data;
        this.currentPage = response.currentPage;
        this.totalPages = response.totalPages;
        this.pageSize = response.pageSize;
        this.totalCount = response.totalCount;
      },
      error: (err) => {
        console.error("Error cargando historial")
      }
    });
  }

  public previousPage(): void {
    this.currentPage = this.currentPage - 1;
    this.getHistory();
  }
  public nextPage(): void {
    this.currentPage = this.currentPage + 1;
    this.getHistory();
  }
}
