import { Component, OnInit } from '@angular/core';
import { ProfileService } from './service/profile.service';
import { AuthService } from '../auth/services/auth.service';
import { UserProfileDto } from './models/user.model';
import { HistorySongsDto, PaginatedResponse } from './models/history.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  userId: number | null = null;
  userProfile?: UserProfileDto;
  profileForm!: FormGroup;

  history: HistorySongsDto[] = [];
  currentPage: number = 1;
  totalPages: number = 0;
  pageSize: number = 8;
  totalCount: number = 0;

  errorMessage: string | null = null;
  successMessage: string | null = null;
  historyLoadingError: boolean = false; 

  constructor(
    private profileService: ProfileService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
    this.authService.getMe().subscribe({
      next: (user) => {
        this.userId = user.id;
        this.loadUserProfile();
        this.getHistory();
      },
      error: (err) => {
        console.error("Error al obtener el usuario loggeado", err);
        this.errorMessage = "No se pudo cargar la información del usuario.";
      }
    });
  }

  private initializeForm(): void {
    this.profileForm = this.fb.group({
      id: [{ value: undefined, disabled: true }],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      repeatPassword: ['', Validators.required],
      roleId: ['']
    });
  }

  onSubmit(): void {
    this.markFormAsTouched();
    if (this.profileForm.valid && this.passwordsMatch) {
      const userToSave = this.createFromForm();
      this.saveUser(userToSave);
    } else {
      console.error('Formulario inválido');
    }
  }

  private markFormAsTouched(): void {
    Object.values(this.profileForm.controls).forEach(control => control.markAsTouched());
  }

  get passwordsMatch(): boolean {
    const { password, repeatPassword } = this.profileForm.value;
    return password === repeatPassword;
  }

  private createFromForm(): UserProfileDto {
    const { id, name, lastName, email, password, roleId, roleName } = this.profileForm.value;
    return {
      id: this.userProfile?.id,
      name,
      lastName,
      email,
      password,
      roleId: this.userProfile?.roleId,
      roleName: this.userProfile?.roleName 
    };
  }

  private saveUser(userToSave: UserProfileDto): void {
    this.profileService.updateUserProfile(userToSave).subscribe({
      next: () => {
        this.successMessage = "Usuario actualizado correctamente.";
        this.loadUserProfile();
      },
      error: (err) => {
        this.errorMessage = "Error al actualizar el usuario.";
        console.error("Error al actualizar el usuario", err);
      }
    });
  }

  private loadUserProfile(): void {
    this.profileService.getUserById().subscribe({
      next: (userProfile) => {
        this.userProfile = userProfile;
        this.profileForm.patchValue({
          id: userProfile.id,
          name: userProfile.name,
          lastName: userProfile.lastName,
          email: userProfile.email,
          password: userProfile.password,
          repeatPassword: userProfile.password
        });
      },
      error: (err) => {
        console.error("Error cargando el perfil de usuario", err);
        this.errorMessage = "No se pudo cargar el perfil de usuario.";
      }
    });
  }

  private getHistory(): void {
    this.profileService.getHistorySongs(this.currentPage, this.pageSize).subscribe({
      next: (response: PaginatedResponse<HistorySongsDto>) => {
        this.history = response.data;
        this.currentPage = response.currentPage;
        this.totalPages = response.totalPages;
        this.pageSize = response.pageSize;
        this.totalCount = response.totalCount;
      },
      error: (err) => {
        this.historyLoadingError = true;
        console.error("Error cargando historial", err);
      }
    });
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage -= 1;
      this.getHistory();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage += 1;
      this.getHistory();
    }
  }
}
