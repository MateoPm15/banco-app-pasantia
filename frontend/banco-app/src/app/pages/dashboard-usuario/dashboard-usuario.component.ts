import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from "../home/home.component";
import { CuentasComponent } from "../cuentas/cuentas.component";
import { MovimientosComponent } from "../movimientos/movimientos.component";

@Component({
  selector: 'app-dashboard-usuario',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HomeComponent, CuentasComponent, MovimientosComponent],
  templateUrl: './dashboard-usuario.component.html',
  styleUrls: ['./dashboard-usuario.component.scss']
})
export class DashboardUsuarioComponent implements OnInit {
  usuario: any = null;
  cuentas: any[] = [];
  movimiento: any[] = [];
  mensaje: string = '';
  error: string = '';
  monto: number = 0;
  cuentaSeleccionada: number | null = null;
  cargandoMovimientos: boolean = true;

  constructor(private apiService: ApiService, private router: Router) {}

  async ngOnInit() {
    await this.cargarDatos();
  }

  async cargarDatos() {
    try {
      // Obtener datos del usuario
      const usuarioResponse = await this.apiService.getUsuarioActual();
      this.usuario = usuarioResponse.data;

      // Obtener cuentas del usuario
      const cuentasResponse = await this.apiService.getCuentas();
      this.cuentas = cuentasResponse.data.filter((c: any) => c.usuarioId === this.usuario.id);

      // Obtener movimientos del usuario asociado
      const movimientosResponse = await this.apiService.getMovimientosUsuario();
      this.movimiento = movimientosResponse.data;
    } catch (error) {
      this.error = 'Error al cargar los datos.';
      console.error(error);
    }
  }

  async realizarCredito() {
    if (!this.cuentaSeleccionada || this.monto <= 0) {
      this.error = 'Seleccione una cuenta y un monto válido.';
      this.autoCerrarNotificacion();
      return;
    }

    try {
      await this.apiService.realizarCredito(this.cuentaSeleccionada, this.monto);
      this.mensaje = 'Crédito realizado correctamente.';
      this.error = '';
      await this.cargarDatos(); // Recargar datos después de la transacción
      this.autoCerrarNotificacion();
    } catch (error: any) {
      this.error = error.message || 'Error realizando crédito.';
      this.autoCerrarNotificacion();
      console.error(error);
    }
  }

  async realizarDebito() {
    if (!this.cuentaSeleccionada || this.monto <= 0) {
      this.error = 'Seleccione una cuenta y un monto válido.';
      this.autoCerrarNotificacion();
      return;
    }

    try {
      await this.apiService.realizarDebito(this.cuentaSeleccionada, this.monto);
      this.mensaje = 'Débito realizado correctamente.';
      this.error = '';
      await this.cargarDatos(); // Recargar datos después de la transacción
      this.autoCerrarNotificacion();
    } catch (error: any) {
      this.error = error.message || 'Error realizando débito.';
      console.error(error);
    }
  }

  private autoCerrarNotificacion() {
    setTimeout(() => {
      this.mensaje = '';
      this.error = '';
    }, 5000); // Cierra automáticamente después de 5 segundos
  }

  cerrarSesion() {
    this.apiService.logout();
    this.router.navigate(['/login']);
  }
}
