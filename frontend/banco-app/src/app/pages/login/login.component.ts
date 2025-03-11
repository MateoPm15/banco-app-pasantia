import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  credentials = { email: '', password: '' };
  errorMessage = '';
  loading = false; // Nueva variable para indicar carga

  constructor(private auth: AuthService, private router: Router) {}

  async login() {
    if (this.loading) return;
    this.loading = true;
    this.errorMessage = '';

    try {
      await this.auth.login(this.credentials.email, this.credentials.password);
    } catch (error: any) {
      this.errorMessage = error.response?.data?.message || 'Credenciales incorrectas';
    } finally {
      this.loading = false;
    }
  }
}
