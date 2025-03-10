import { Component, Input, OnInit} from '@angular/core';
import { ApiService } from '../../services/api.service';
import { CommonModule } from '@angular/common';

interface Movimiento {
  id: number;
  fecha: Date;
  tipoMovimiento: string;
  valor: number;
  saldo: number;
  cuentaId: number;
}
@Component({
  selector: 'app-movimientos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './movimientos.component.html',
  styleUrl: './movimientos.component.scss'
})
export class MovimientosComponent {
  @Input() movimientos: Movimiento[] = [];
}
