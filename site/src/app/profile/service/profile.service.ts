import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';
import { HistorySongsDto, PaginatedResponse } from '../models/history.model';
import { Observable, switchMap } from 'rxjs';
import { UserProfileDto } from '../models/user.model';

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

  public getHistorySongs(page: number, size: number): Observable<PaginatedResponse<HistorySongsDto>> {
    return this.authService.getMe().pipe(
      switchMap(user => {
        const userId = user.id;
        const urlEndPoint = `${this.apiUrl}/user/${userId}/history?PageNumber=${page}&PageSize=${size}`;
        return this.http.get<PaginatedResponse<HistorySongsDto>>(urlEndPoint, { headers: this.getAuthHeaders() });
      })
    );
  }

  public getUserById(): Observable<UserProfileDto> {
    return this.authService.getMe().pipe(
      switchMap(user => {
        const userId = user.id;
        const urlEndPoint = `${this.apiUrl}/users/${userId}`;
        return this.http.get<UserProfileDto>(urlEndPoint, { headers: this.getAuthHeaders() });
      })
    );
  }

  public updateUserProfile(updatedProfile: UserProfileDto): Observable<UserProfileDto> {
    return this.authService.getMe().pipe(
      switchMap(user => {
        const userId = user.id;
        const urlEndPoint = `${this.apiUrl}/users`;
        return this.http.put<UserProfileDto>(urlEndPoint, updatedProfile, { headers: this.getAuthHeaders() });
      })
    );
  }
}
