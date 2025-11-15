import { Injectable, signal } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'vibe_travels_token';
  
  private accessToken = signal<string | undefined>(undefined);
  private authStateSubject = new BehaviorSubject<boolean>(false);
  
  public authState$: Observable<boolean> = this.authStateSubject.asObservable();

  constructor() {
    this.loadTokenFromStorage();
  }

  getToken = (): string | undefined => this.accessToken();

  setToken(token: string): void {
    this.accessToken.set(token);
    localStorage.setItem(this.TOKEN_KEY, token);
    this.authStateSubject.next(true);
  }

  clearToken(): void {
    this.accessToken.set(undefined);
    localStorage.removeItem(this.TOKEN_KEY);
    this.authStateSubject.next(false);
  }

  isAuthenticated = (): boolean => this.accessToken() !== undefined;

  private loadTokenFromStorage(): void {
    const token = localStorage.getItem(this.TOKEN_KEY);

    if (token) {
      this.accessToken.set(token);
      this.authStateSubject.next(true);
    }
  }
}
