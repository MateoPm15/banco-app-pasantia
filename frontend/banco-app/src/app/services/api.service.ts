import { Injectable } from '@angular/core';
import axios, { AxiosResponse } from 'axios';
import { TokenService } from './token.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'http://localhost:5149/api';

  constructor(private tokenService: TokenService, private router: Router) {}

  private getAuthHeader() {
    const token = this.tokenService.getToken();
    return token ? { headers: { Authorization: `Bearer ${token}` } } : {};
  }

  async getUsuarios(): Promise<AxiosResponse<any>> {
    try {
      return await axios.get(`${this.apiUrl}/Usuarios`, this.getAuthHeader());
    } catch (error) {
      console.error('Error obteniendo usuarios:', error);
      throw error;
    }
  }

  async getUsuarioActual(): Promise<AxiosResponse<any>> {
    try {
      const response = await axios.get(`${this.apiUrl}/Usuarios/me`, this.getAuthHeader());
      return response;
    } catch (error) {
      console.error('Error obteniendo usuario actual:', error);
      throw error;
    }
  }

  async getCuentas(): Promise<AxiosResponse<any>> {
    try {
      const response = await axios.get(`${this.apiUrl}/Cuentas`, this.getAuthHeader());
      return response;
    } catch (error) {
      console.error('Error obteniendo cuentas:', error);
      throw error;
    }
  }

  async getMovimientos(): Promise<AxiosResponse<any>> {
    try {
      const response = await axios.get(`${this.apiUrl}/Movimientos`, this.getAuthHeader());
      return response;
    } catch (error) {
      console.error('Error obteniendo movimientos:', error);
      throw error;
    }
  }

  async getMovimientosUsuario(): Promise<AxiosResponse<any>> {
    try {
      const response = await axios.get(
        `${this.apiUrl}/Movimientos/usuario`,
        this.getAuthHeader()
      );
      return response;
    } catch (error) {
      console.error('Error obteniendo movimientos del usuario:', error);
      throw error;
    }
  }

  async realizarCredito(idCuenta: number, monto: number): Promise<AxiosResponse<any>> {
    if (monto > 3000) {
      throw new Error('El valor del crédito no puede ser mayor a 3000.');
    }
    try {
      const response = await axios.post(
        `${this.apiUrl}/Movimientos/credito`,
        { CuentaId: idCuenta, Valor: monto },
        this.getAuthHeader()
      );
      return response;
    } catch (error) {
      console.error('Error realizando crédito:', error);
      throw error;
    }
  }

  async realizarDebito(idCuenta: number, monto: number): Promise<AxiosResponse<any>> {
    try {
      const response = await axios.post(
        `${this.apiUrl}/Movimientos/debito`,
        { CuentaId: idCuenta, Valor: monto },
        this.getAuthHeader()
      );
      return response;
    } catch (error: any) {
      if (error.response?.status === 400) {
        throw new Error('Saldo insuficiente para realizar esta transacción.');
      }
      console.error('Error realizando débito:', error);
      throw error;
    }
  }

  async login(data: { Email: string; Password: string }): Promise<AxiosResponse<any>> {
    try {
      const response = await axios.post(`${this.apiUrl}/auth/login`, data);
      return response;
    } catch (error) {
      console.error('Error en el login:', error);
      throw error;
    }
  }

  logout(): void {
    this.tokenService.clearToken();
    this.router.navigate(['/login']);
  }
}
