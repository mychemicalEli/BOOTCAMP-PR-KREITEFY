import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, switchMap } from 'rxjs';
import { LatestSongsDto } from '../models/lastest.model';
import { SongDetailDto } from '../models/song-detail.model';
import { RatingDto } from '../models/rating.model';
import { AuthService } from '../../auth/services/auth.service';
import { YourSongsDto } from '../models/yourSongs.model';
import { MostPlayedSongsDto } from '../models/most-played.model';
import { SongListComponent } from '../song-list/song-list.component';
import { PaginatedResponse, SongListDto } from '../models/all-songs.model';

@Injectable({
  providedIn: 'root'
})
export class SongService {
  private apiUrl = 'http://localhost:5282';

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

  public getMostPlayedSongs(genreId?: number): Observable<MostPlayedSongsDto[]> {
    let urlEndPoint = `${this.apiUrl}/songs/most-played`;
    if (genreId !== undefined) {
      urlEndPoint += `?genreId=${genreId}`;
    }
    return this.http.get<MostPlayedSongsDto[]>(urlEndPoint, { headers: this.getAuthHeaders() });
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

  public getAllSongs(page: number, size: number, filters?: string): Observable<PaginatedResponse<SongListDto>> {
    let urlEndPoint = `${this.apiUrl}/songs?PageNumber=${page}&PageSize=${size}`;
    if (filters) {
      urlEndPoint += `&filter=${filters}`;
    }
    return this.http.get<PaginatedResponse<SongListDto>>(urlEndPoint, { headers: this.getAuthHeaders() });
  }

  public getGenres(): Observable<{ id: number; name: string }[]> {
    const urlEndPoint = `${this.apiUrl}/genres`;
    return this.http.get<{ id: number; name: string }[]>(urlEndPoint, { headers: this.getAuthHeaders() });
  }

  public getAlbums(): Observable<{ id: number; title: string }[]> {
    const urlEndPoint = `${this.apiUrl}/albums`;
    return this.http.get<{ id: number; title: string }[]>(urlEndPoint, { headers: this.getAuthHeaders() });
  }

  public getArtists(): Observable<{ id: number; name: string }[]> {
    const urlEndPoint = `${this.apiUrl}/artists`;
    return this.http.get<{ id: number; name: string }[]>(urlEndPoint, { headers: this.getAuthHeaders() });
  }

}
