import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../shared/spinner/service/loading.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm!: FormGroup;
  errorMessage: string | null = null;
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private loadingService: LoadingService
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
    this.loadingService.loading$.subscribe((isLoading) => {
      this.loading = isLoading;
    });
  }
  private initializeForm(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      password: ['', [Validators.required]],
    });
  }

  onSubmit(): void {
    this.markFormAsTouched();
    if (this.loginForm.valid) {
      const credentials = this.createFromForm();
      this.loadingService.show();
      this.authenticateUser(credentials);
    } else {
      console.error('Formulario inválido');
    }
  }

  private markFormAsTouched(): void {
    Object.values(this.loginForm.controls).forEach(control => control.markAsTouched());
  }

  private createFromForm(): { email: string; password: string } {
    const { email, password } = this.loginForm.value;
    return { email, password };
  }

  private handleError(error: any): void {
    console.error(error);
    this.errorMessage = error.status === 401
      ? "Credenciales incorrectas. Por favor, verifica tu email y contraseña."
      : "Ocurrió un error inesperado. Inténtalo de nuevo.";
    this.loadingService.hide();
  }

  private authenticateUser(credentials: { email: string; password: string }): void {
    this.authService.login(credentials).subscribe({
      next: (response) => {
        const token = response.token;
        this.authService.saveToken(token);
        this.loadingService.hide();
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.handleError(err);
      }
    });
  }

}
