import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, delay } from 'rxjs';
import { UserDtoResponse } from '../models/userdto.response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5282';
  private loggedInSubject = new BehaviorSubject<boolean>(this.isAuthenticated());
  private userSubject = new BehaviorSubject<UserDtoResponse | null>(null);

  constructor(private http: HttpClient) { }

  // Registro de usuario
  register(user: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/register`, user).pipe(
      tap((response: any) => {
        this.saveToken(response.token);
      })
    );
  }

  // Login de usuario
  login(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/login`, credentials).pipe(
      tap((response: any) => {
        this.saveToken(response.token);
      })
    );
  }

  // Obtener los datos del usuario autenticado
  getMe(): Observable<UserDtoResponse> {
    const token = this.getToken();
    if (!token) throw new Error('No token found');

    return this.http.get<UserDtoResponse>(`${this.apiUrl}/auth/me`, {
      headers: { Authorization: `Bearer ${token}` }
    }).pipe(
      tap(user => this.userSubject.next(user))
    );
  }

  // Guardar token en el localStorage
  public saveToken(token: string): void {
    if (this.isBrowser()) {
      localStorage.setItem('authToken', token);
      this.loggedInSubject.next(true);
    }
  }

  // Obtener token desde el localStorage
  public getToken(): string | null {
    return this.isBrowser() ? localStorage.getItem('authToken') : null;
  }

  // Verificar si el usuario está autenticado
  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  // Logout del usuario
  logout(): void {
    if (this.isBrowser()) {
      localStorage.removeItem('authToken');
      this.loggedInSubject.next(false);
      this.userSubject.next(null);
    }
  }

  // Métodos observables para que los componentes se suscriban
  getLoggedInStatus(): Observable<boolean> {
    return this.loggedInSubject.asObservable();
  }

  getUserObservable(): Observable<UserDtoResponse | null> {
    return this.userSubject.asObservable();
  }

  // Método para verificar si está ejecutándose en un entorno de navegador
  private isBrowser(): boolean {
    return typeof window !== 'undefined' && typeof window.localStorage !== 'undefined';
  }
}
