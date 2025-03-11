import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { DashboardUsuarioComponent } from './pages/dashboard-usuario/dashboard-usuario.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'dashboard-usuario', component: DashboardUsuarioComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: 'login' }
];
