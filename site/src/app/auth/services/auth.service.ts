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
  private userNameSubject = new BehaviorSubject<string>(this.getDecodedUserInfo().userName || '');
  private userIdSubject = new BehaviorSubject<string | null>(this.getDecodedUserInfo().userId);
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

  // Guardar token en el localStorage y actualizar el usuario decodificado
  saveToken(token: string): void {
    if (this.isBrowser()) {
      localStorage.setItem('authToken', token);
      
      // Obtener tanto el userName como el userId desde el token decodificado
      const { userName, userId } = this.getDecodedUserInfo();
      
      // Guardar los valores en localStorage
      localStorage.setItem('username', userName || '');
      localStorage.setItem('userId', userId || '');
      
      // Actualizar los BehaviorSubjects
      this.loggedInSubject.next(true);
      this.userNameSubject.next(userName || '');
      this.userIdSubject.next(userId || null);
    }
  }
  
  // Obtener token desde el localStorage
  getToken(): string | null {
    return this.isBrowser() ? localStorage.getItem('authToken') : null;
  }

  // Obtener el nombre de usuario y el id decodificando el token
  public getDecodedUserInfo(): { userId: string | null, userName: string | null } {
    const token = this.getToken();
    if (token) {
      try {
        const payload = token.split('.')[1];
        const decodedPayload = JSON.parse(decodeURIComponent(atob(payload)));
  
        const userName = decodedPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] || null;
        const userId = decodedPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || null;
  
        return { userId, userName };
      } catch (e) {
        console.error('Error decoding token:', e);
        return { userId: null, userName: null };
      }
    }
    return { userId: null, userName: null };
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
