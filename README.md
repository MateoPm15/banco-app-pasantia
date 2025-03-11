# Banco CODESARROLLO - Aplicación Bancaria para Pasantía

[![GitHub Repo](https://img.shields.io/badge/Repositorio-GitHub-blue)](https://github.com/MateoPm15/banco-app-pasantia/tree/development)

Aplicación web desarrollada como parte del proceso de selección para una pasantía en desarrollo de software. Cumple con los requisitos de gestión de entidades (Persona, Usuario, Cuenta, Movimientos), operaciones CRUD, validaciones específicas y autenticación de usuarios.

---

## 📋 Estructura del Proyecto

```bash
mateopm15-banco-app-pasantia/
├── backend/               # API .NET 9.0 con Entity Framework Core
├── frontend/              # Aplicación Angular con autenticación JWT
├── database/              # Configuración de SQL Server en Docker
└── ...                    # Configuraciones adicionales
```

---

## 🛠️ Herramientas Utilizadas

- **Backend**: 
  - .NET 9.0
  - Entity Framework Core (SQL Server)
  - JWT para autenticación
  - Swagger/Postman para pruebas
- **Frontend**: 
  - Angular 17
  - Angular Material
  - Bootstrap
  - NgxToastr para notificaciones
- **Base de Datos**: 
  - SQL Server 2019 (Docker)
  - Migraciones con EF Core

---

## Instalación y Configuración

### Requisitos Previos
- .NET SDK 9.0
- Node.js 18+
- Docker Desktop
- Angular CLI
- Postman (opcional)

### Pasos para Ejecutar el Proyecto

1. **Clonar el Repositorio**:
   ```bash
   git clone https://github.com/MateoPm15/banco-app-pasantia.git
   cd banco-app-pasantia
   ```

2. **Configurar la Base de Datos**:
   ```bash
   cd database
   docker-compose up -d  # Inicia SQL Server en Docker
   ```

3. **Configurar y Ejecutar el Backend**:
   ```bash
   cd ../backend/BancoAPI
   dotnet restore
   dotnet ef database update  # Aplica migraciones
   dotnet run  # Servidor en http://localhost:5000
   ```

4. **Configurar y Ejecutar el Frontend**:
   ```bash
   cd ../../frontend/banco-app
   npm install
   ng serve  # Aplicación en http://localhost:4200
   ```

---

## 📚 Funcionalidades Clave

### Backend (API .NET)
- **Endpoints CRUD** para:
  - Usuarios (heredan de Persona)
  - Cuentas (Ahorros/Corriente)
  - Movimientos (Crédito/Débito)
- **Validaciones**:
  - Saldo mínimo para débitos.
  - Límite de $3000 para créditos.
- **Autenticación JWT** con roles.

### Frontend (Angular)
- **Inicio de Sesión**:
  - Validación de credenciales.
  - Almacenamiento seguro de tokens.
- **Dashboard de Usuario**:
  - Consulta de saldo.
  - Registro de movimientos.
- **Validaciones en Tiempo Real**:
  - Mensajes de error para saldo insuficiente.
  - Restricción de montos mayores a $3000.

---

## 🧪 Pruebas con Postman

1. **Crear Usuario**:
   ```http
   POST http://localhost:5149/api/Usuarios
   Body (JSON):
    {
    "Nombre": "Moni",
    "Apellido": "Pilco",
    "Direccion": "Calle 123",
    "Email": "pato@example.com",
    "Contrasenia": "moni123",
    "Estado": true
    }
   ```

2. **Realizar Movimiento**:
   ```http
   POST http://localhost:5149/api/Movimientos/credito
   Body (JSON):
    {
      "Valor": 500,
      "CuentaId": "638770829410435413"
    }
   ```

---

## ✅ Buenas Prácticas Implementadas

- **Backend**:
  - Inyección de dependencias.
  - Migraciones automatizadas con EF Core.
  - Manejo centralizado de excepciones.
- **Frontend**:
  - Guards para rutas protegidas.
  - Interceptores HTTP para manejo de errores.
  - Componentes reutilizables (navbar, loading).

---

## 📄 Entregables

- [Repositorio Público](https://github.com/MateoPm15/banco-app-pasantia/tree/development)
- Scripts de Base de Datos (Migrations/).
- Documentación técnica en código.

---

## 📧 Contacto

**Mateo Pilco**  
📧 mateo.pilco.dev@gmail.com  
🔗 [LinkedIn](https://www.linkedin.com/in/mateo-pilco-1703611a9/)  
🛠️ Listo para demostrar el proyecto en entrevista técnica.