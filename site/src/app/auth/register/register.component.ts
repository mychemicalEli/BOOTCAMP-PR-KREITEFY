import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { RegisterRequest } from '../models/register.request.model';
import { Router } from '@angular/router';
import { passwordValidator } from '../../shared/validator/password.validator';
import { LoadingService } from '../../shared/spinner/service/loading.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm!: FormGroup;
  errorMessage: string | null = null;
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: Router,
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
    this.registerForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      password: ['', [Validators.required, passwordValidator()]],
      repeatPassword: ['', [Validators.required]],
    });
  }

  onSubmit(): void {
    this.markFormAsTouched();
    if (this.registerForm.valid && this.passwordsMatch) {
      this.saveUser(this.createFromForm());
      this.loadingService.show();
    } else {
      console.error('Formulario inválido');
    }
  }

  private markFormAsTouched(): void {
    Object.values(this.registerForm.controls).forEach(control => control.markAsTouched());
  }

  get passwordsMatch(): boolean {
    const { password, repeatPassword } = this.registerForm.value;
    return password === repeatPassword;
  }

  private createFromForm(): RegisterRequest {
    const { name, lastName, email, password } = this.registerForm.value;
    return {
      id: undefined,
      name,
      lastName,
      email,
      password,
      roleId: 2,
      roleName: '',
      token: '',
    };
  }

  private handleError(error: any): void {
    console.log(error);
    if (error.status === 400) {
      this.errorMessage = error.error?.message || "Ya existe un usuario con este email. ¡Prueba con otro!";
    } else {
      this.errorMessage = "Ocurrió un error inesperado. Inténtalo de nuevo.";
      this.loadingService.hide();
    }
  }

  private saveUser(userToSave: RegisterRequest): void {
    this.authService.register(userToSave).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token);
        this.loadingService.hide();
        this.route.navigate(['/']);
      },
      error: (err) => {
        this.handleError(err);
      }
    });
  }
}
