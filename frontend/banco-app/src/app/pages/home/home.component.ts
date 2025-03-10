import { Component, Input, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { CommonModule } from '@angular/common';

// Interface recomendada
export interface Usuario {
  id: number;
  nombre: string;
  apellido: string;
  direccion: string;
  email: string;
  // Agregar más campos según tu API
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  @Input() usuario?: Usuario; // Usuario individual
  usuarios: Usuario[] = [];   // Lista completa

  constructor(private apiService: ApiService) {}

  async ngOnInit() {
    if (!this.usuario) {
      await this.cargarTodosUsuarios();
    }
  }

  private async cargarTodosUsuarios() {
    try {
      const response = await this.apiService.getUsuarios();
      this.usuarios = response.data;
    } catch (error) {
      console.error('Error cargando usuarios:', error);
    }
  }
}
