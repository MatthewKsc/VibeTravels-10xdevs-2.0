import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environments';
import { SignIn, SignUp, Jwt } from './auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthApiService {
  private httpClient = inject(HttpClient);
  private readonly authApiUrl = environment.apiUrl + '/users';

  signUp(command: SignUp): Observable<void> {
    return this.httpClient.post<void>(`${this.authApiUrl}/signup`, command);
  }

  signIn(command: SignIn): Observable<Jwt> {
    return this.httpClient.post<Jwt>(`${this.authApiUrl}/signin`, command);
  }
}
