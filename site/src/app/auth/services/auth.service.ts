import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { RegisterRequest } from "../models/register.request.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5282/api';
  private loggedInSubject = new BehaviorSubject<boolean>(this.isAuthenticated());
  private userNameSubject = new BehaviorSubject<string>(this.getUserName() || '');

  constructor(private http: HttpClient) { }

  // Registro de usuario
  public register(user: RegisterRequest): Observable<RegisterRequest> {
    return this.http.post<RegisterRequest>(`${this.apiUrl}/auth/register`, user);
  }

  // Login de usuario
  public login(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/login`, credentials);
  }

  // Guardar token y nombre de usuario en el localStorage
  saveToken(token: string, name: string): void {
    if (this.isBrowser()) {
      localStorage.setItem('authToken', token);
      localStorage.setItem('username', name);
      this.loggedInSubject.next(true);
      this.userNameSubject.next(name);
    }
  }

  // Obtener token desde el localStorage
  getToken(): string | null {
    return this.isBrowser() ? localStorage.getItem('authToken') : null;
  }

  // Obtener nombre de usuario desde el localStorage
  getUserName(): string | null {
    return this.isBrowser() ? localStorage.getItem('username') : null;
  }

  // Verificar si el usuario está autenticado
  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  // Logout del usuario
  logout(): void {
    if (this.isBrowser()) {
      localStorage.removeItem('authToken');
      localStorage.removeItem('username');
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
