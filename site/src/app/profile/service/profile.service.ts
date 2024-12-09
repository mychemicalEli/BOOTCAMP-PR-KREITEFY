import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';
import { HistorySongsDto, PaginatedResponse } from '../models/history.model';
import { Observable, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  private apiUrl = 'http://localhost:5282';

  constructor(private http: HttpClient, private authService: AuthService) { }

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  // Función para obtener el historial de canciones del usuario logueado
  public getHistorySongs(page: number, size: number): Observable<PaginatedResponse<HistorySongsDto>> {
    return this.authService.getMe().pipe(
      switchMap(user => {
        const userId = user.id; // Obtén el userId del usuario logueado
        const urlEndPoint = `${this.apiUrl}/user/${userId}/history?PageNumber=${page}&PageSize=${size}`;
        return this.http.get<PaginatedResponse<HistorySongsDto>>(urlEndPoint, { headers: this.getAuthHeaders() });
      })
    );
  }
}
