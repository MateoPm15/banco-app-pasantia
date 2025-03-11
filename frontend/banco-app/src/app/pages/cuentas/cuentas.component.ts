import { Component, Input, OnInit} from '@angular/core';
import { ApiService } from '../../services/api.service';
import { CommonModule } from '@angular/common';

interface Cuenta {
  id: number;
  numeroCuenta: string;
  saldo: number;
  tipoCuenta: 'Ahorro' | 'Corriente';
  estado: boolean;
  usuarioId: number;
}

@Component({
  selector: 'app-cuentas',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cuentas.component.html',
  styleUrl: './cuentas.component.scss'
})
export class CuentasComponent {
  @Input() cuentas: Cuenta[] = [];
}
