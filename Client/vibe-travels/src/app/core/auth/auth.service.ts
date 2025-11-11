import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private accessToken = signal<string | undefined>(undefined);

  getToken = (): string | undefined => this.accessToken();

  setToken = (token: string): void => this.accessToken.set(token);

  clearToken = (): void => this.accessToken.set(undefined);

  isAuthenticated = (): boolean =>this.accessToken() !== undefined;

  setDevelopmentToken(): void {
    const devToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI4ODc4Y2IxYi02YTc3LTRiYWItOWE1MC0xMTc2Y2UwMzIyZjQiLCJlbWFpbCI6InVzZXIxQGV4YW1wbGUuY29tIiwianRpIjoiZjBlN2IxNDMtMzNmMC00YzhlLWEzZDQtMTVlNWE5NThjYTc4IiwiZXhwIjoxNzYyODg0NTM3LCJpc3MiOiJWaWJlVHJhdmVscyIsImF1ZCI6IlZpYmVUcmF2ZWxzIn0.bQCvptouAVJJfMap1LbX7qdm6T7ktxJCj2x88gPiTQk'
    this.setToken(devToken);
  }
}
