import { Component, OnInit } from "@angular/core";
import { AuthService } from "../../auth/services/auth.service";
import { Router } from "@angular/router";

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
    });

    this.authService.getUserNameObservable().subscribe(name => {
      this.userName = name;
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  handleLoginLogout(): void {
    if (this.isLoggedIn) {
      this.logout(); // Si está logueado, hacemos logout
    } else {
      this.router.navigate(['/auth/login']); // Si no está logueado, redirigimos a Login
    }
  }
}
