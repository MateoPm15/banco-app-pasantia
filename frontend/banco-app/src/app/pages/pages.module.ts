import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { CuentasComponent } from './cuentas/cuentas.component';
import { MovimientosComponent } from './movimientos/movimientos.component';
import { LoginComponent } from './login/login.component';
import { DashboardUsuarioComponent } from './dashboard-usuario/dashboard-usuario.component';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HomeComponent, //Importamos el componente standalone
    CuentasComponent,
    MovimientosComponent,
    LoginComponent,
    DashboardUsuarioComponent
  ]
})
export class PagesModule { }
