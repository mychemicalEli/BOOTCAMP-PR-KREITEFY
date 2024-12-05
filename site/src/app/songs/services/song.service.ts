import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, switchMap } from 'rxjs';
import { LatestSongsDto } from '../models/lastest.model';
import { SongDetailDto } from '../models/song-detail.model';
import { RatingDto } from '../models/rating.model';
import { AuthService } from '../../auth/services/auth.service';
import { YourSongsDto } from '../models/yourSongs.model';

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

  public getLatestSongs(genreId?: number): Observable<LatestSongsDto[]> {
    let urlEndPoint = `${this.apiUrl}/songs/latest`;
    if (genreId !== undefined) {
      urlEndPoint += `?genreId=${genreId}`;
    }
    return this.http.get<LatestSongsDto[]>(urlEndPoint, { headers: this.getAuthHeaders() });
  }

  public getForYouSongs(): Observable<YourSongsDto[]> {
    return this.authService.getMe().pipe(
      switchMap(user => {
        const urlEndPoint = `${this.apiUrl}/user/${user.id}/songsforyou`;
        return this.http.get<YourSongsDto[]>(urlEndPoint, { headers: this.getAuthHeaders() });
      })
    );
  }

  public getSongById(songId: number): Observable<SongDetailDto> {
    const urlEndPoint = `${this.apiUrl}/songs/${songId}`;
    return this.http.get<SongDetailDto>(urlEndPoint, { headers: this.getAuthHeaders() });
  }

  public incrementStreams(songId: number, userId: number): Observable<any> {
    const urlEndPoint = `${this.apiUrl}/play`;
    // Enviar el songId y userId como parámetros de consulta
    return this.http.post(urlEndPoint, null, { 
      headers: this.getAuthHeaders(),
      params: { songId: songId.toString(), userId: userId.toString() }
    });
  }
  

  public addRating(ratingDto: RatingDto): Observable<any> {
    const urlEndPoint = `${this.apiUrl}/rating`;
    return this.http.post(urlEndPoint, ratingDto, { headers: this.getAuthHeaders() });
  }

  public getGenres(): Observable<{ id: number; name: string }[]> {
    const urlEndPoint = `${this.apiUrl}/genres`;
    return this.http.get<{ id: number; name: string }[]>(urlEndPoint, { headers: this.getAuthHeaders() });
  }
  
}
