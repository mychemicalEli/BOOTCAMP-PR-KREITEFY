import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ArtistDto } from '../models/artists.model';
import { Observable } from 'rxjs';
import { AlbumDto } from '../models/album.model';
import { LatestSongs } from '../models/latest.model';

@Injectable({
  providedIn: 'root'
})
export class SongService {
  private apiUrl = 'http://localhost:5282/api';
  constructor(private http: HttpClient) { }

  public getArtist(): Observable<ArtistDto[]> {
    const urlEndPoint = `${this.apiUrl}/artists`;
    return this.http.get<ArtistDto[]>(urlEndPoint);
  }

  public getAlbums(): Observable<AlbumDto[]> {
    const urlEndPoint = `${this.apiUrl}/albums`;
    return this.http.get<AlbumDto[]>(urlEndPoint);
  }

  public getLatestSongs(): Observable<LatestSongs[]>{
    const urlEndPoint = `${this.apiUrl}/songs/latest`;
    return this.http.get<LatestSongs[]>(urlEndPoint);
  }
}
