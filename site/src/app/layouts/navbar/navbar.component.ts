import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../auth/services/auth.service";
import { Router } from "@angular/router";
import { UserDtoResponse } from "../../auth/models/userdto.response";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  isLoggedIn: boolean = false;
  userName: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.getLoggedInStatus().subscribe(status => {
      this.isLoggedIn = status;
      if (this.isLoggedIn) {
        this.loadUserData();
      } else {
        this.userName = '';
      }
    });
    this.authService.getUserObservable().subscribe(user => {
      if (user) {
        this.userName = user.name;
      }
    });
  }

  loadUserData(): void {
    this.authService.getMe().subscribe(
      (user: UserDtoResponse) => {
        this.userName = user.name;
      },
      (error) => {
        console.error('Error al obtener los datos del usuario:', error);
      }
    );
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  handleLoginLogout(): void {
    if (this.isLoggedIn) {
      this.logout();
    } else {
      this.router.navigate(['/auth/login']);
    }
  }
}
