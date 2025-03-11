import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly USER_KEY = 'current_user';
  private readonly TOKEN_KEY = 'auth_token';
  private isBrowser: boolean;

  constructor(@Inject(PLATFORM_ID) platformId: Object) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  setToken(token: string): void {
    if (this.isBrowser) localStorage.setItem(this.TOKEN_KEY, token);
  }

  getToken(): string | null {
    return this.isBrowser ? localStorage.getItem(this.TOKEN_KEY) : null;
  }

  setUserData(userData: any): void {
    if (this.isBrowser) {
      localStorage.setItem(this.USER_KEY, JSON.stringify(userData));
    }
  }

  getUserData(): any {
    if (this.isBrowser) {
      const data = localStorage.getItem(this.USER_KEY);
      return data ? JSON.parse(data) : null;
    }
    return null;
  }

  clearToken(): void {
    if (this.isBrowser) {
      localStorage.removeItem(this.TOKEN_KEY);
      localStorage.removeItem(this.USER_KEY);
    }
  }
}
