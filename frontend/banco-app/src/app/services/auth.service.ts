import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from './api.service';
import { BehaviorSubject } from 'rxjs';
import { TokenService } from './token.service';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<any>(null);
  currentUser$ = this.currentUserSubject.asObservable();
  private isBrowser: boolean;

  constructor(
    private api: ApiService,
    private tokenService: TokenService,
    private router: Router,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    this.safeLoadUserFromStorage();
  }

  private safeLoadUserFromStorage() {
    if (this.isBrowser) {
      const token = this.tokenService.getToken();
      if (token) {
        const userData = this.tokenService.getUserData();
        if (userData) this.currentUserSubject.next(userData);
      }
    }
  }

  async login(email: string, password: string) {
    if (!this.isBrowser) return;

    try {
      const response = await this.api.login({ Email: email, Password: password });
      if (response.data?.token) {
        this.tokenService.setToken(response.data.token);
        const userData = {
          id: response.data.ID,
          nombre: response.data.Nombre,
          email: response.data.Email
        };
        this.tokenService.setUserData(userData);
        this.currentUserSubject.next(userData);
        this.router.navigate(['/dashboard-usuario']);
      }
      return response;
    } catch (error) {
      console.error('Error en login:', error);
      throw error;
    }
  }

  logout() {
    if (this.isBrowser) {
      this.tokenService.clearToken();
      this.currentUserSubject.next(null);
      this.router.navigate(['/login']);
    }
  }

  isAuthenticated(): boolean {
    return this.isBrowser ? !!this.tokenService.getToken() : false;
  }

  getCurrentUser() {
    return this.currentUserSubject.value;
  }
}
