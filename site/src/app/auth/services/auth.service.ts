import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegisterRequest } from '../models/register.request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5282/api/auth/register';
  constructor(private http: HttpClient) { }

  public register(user: RegisterRequest): Observable<RegisterRequest> {
    return this.http.post<RegisterRequest>(this.apiUrl, user);
  }
}
