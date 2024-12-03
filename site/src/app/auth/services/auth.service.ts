import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, tap } from "rxjs";
import { RegisterRequest } from "../models/register.request.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5282/api';
  private loggedInSubject = new BehaviorSubject<boolean>(this.isAuthenticated());
  private userNameSubject = new BehaviorSubject<string>(this.getDecodedUserName() || '');

  constructor(private http: HttpClient) { }

  // Registro de usuario
  public register(user: RegisterRequest): Observable<RegisterRequest> {
    return this.http.post<RegisterRequest>(`${this.apiUrl}/auth/register`, user).pipe(
      tap((response: any) => {
        const token = response.token;
        this.saveToken(token);
      })
    );
  }

  // Login de usuario
  public login(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/login`, credentials).pipe(
      tap((response: any) => {
        const token = response.token;
        this.saveToken(token);
      })
    );
  }

  // Guardar token en el localStorage y actualizar el nombre de usuario decodificado
  saveToken(token: string): void {
    if (this.isBrowser()) {
      localStorage.setItem('authToken', token);
      const userName = this.getDecodedUserName();
      localStorage.setItem('username', userName || '');
      this.loggedInSubject.next(true);
      this.userNameSubject.next(userName || '');
    }
  }

  // Obtener token desde el localStorage
  getToken(): string | null {
    return this.isBrowser() ? localStorage.getItem('authToken') : null;
  }

  // Obtener el nombre de usuario decodificando el token
  private getDecodedUserName(): string | null {
    const token = this.getToken();
    if (token) {
      try {
        const payload = token.split('.')[1];
        const decodedPayload = JSON.parse(atob(payload));
        return decodedPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || null; // Acceder al claim correcto
      } catch (e) {
        console.error('Error decoding token:', e);
        return null;
      }
    }
    return null;
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
      this.userNameSubject.next('');
    }
  }

  // Métodos observables para que los componentes se suscriban
  getLoggedInStatus(): Observable<boolean> {
    return this.loggedInSubject.asObservable();
  }

  getUserNameObservable(): Observable<string> {
    return this.userNameSubject.asObservable();
  }

  // Método privado para verificar si está ejecutándose en un entorno de navegador
  private isBrowser(): boolean {
    return typeof window !== 'undefined' && typeof window.localStorage !== 'undefined';
  }
}
