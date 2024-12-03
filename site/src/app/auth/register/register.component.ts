import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { RegisterRequest } from '../models/register.request.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm!: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: Router
  ) {
    this.initializeForm();
  }

  private initializeForm(): void {
    this.registerForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      repeatPassword: ['', Validators.required],
    });
  }

  onSubmit(): void {
    this.markFormAsTouched();
    if (this.registerForm.valid && this.passwordsMatch()) {
      this.saveUser(this.createFromForm());
    } else {
      console.error('Formulario inválido');
    }
  }

  private markFormAsTouched(): void {
    Object.values(this.registerForm.controls).forEach(control => control.markAsTouched());
  }

  passwordsMatch(): boolean {
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
    }
  }

  private saveUser(userToSave: RegisterRequest): void {
    this.authService.register(userToSave).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token);
        localStorage.setItem('username', response.name);
        this.route.navigate(['/']);
      },
      error: (err) => {
        this.handleError(err);
      }
    });
  }
}
