import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  isLoggedIn: boolean = false;
  data: any = null;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.authService.getLoggedInStatus().subscribe(status => {
      this.isLoggedIn = status;
      if (this.isLoggedIn) {
        this.fetchUserData();
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
}
