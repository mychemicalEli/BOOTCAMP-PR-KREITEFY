import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LatestSongs } from '../models/latest.model';
import { SongDto } from '../models/song.model';
import { RatingDto } from '../models/rating.model';
import { AuthService } from '../../auth/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class SongService {
  private apiUrl = 'http://localhost:5282/api';

  constructor(private http: HttpClient, private authService: AuthService) { }

  // Función auxiliar para obtener los headers con el token
  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  public getLatestSongs(): Observable<LatestSongs[]> {
    const urlEndPoint = `${this.apiUrl}/songs/latest`;
    return this.http.get<LatestSongs[]>(urlEndPoint, { headers: this.getAuthHeaders() });
  }

  public getSongById(songId: number): Observable<SongDto> {
    const urlEndPoint = `${this.apiUrl}/songs/${songId}`;
    return this.http.get<SongDto>(urlEndPoint, { headers: this.getAuthHeaders() });
  }

  public incrementStreams(songId: number): Observable<any> {
    const urlEndPoint = `${this.apiUrl}/play`;
    return this.http.post(urlEndPoint, songId, { headers: this.getAuthHeaders() });
  }

  public addRating(ratingDto: RatingDto): Observable<any> {
    const urlEndPoint = `${this.apiUrl}/rating`;
    return this.http.post(urlEndPoint, ratingDto, { headers: this.getAuthHeaders() });
  }
}
