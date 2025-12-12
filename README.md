# PATI - Sistema de Control Integral de Confección

Sistema completo de gestión de producción para empresa de confección, con control de talleres, inventario, calidad y pagos.

## 📁 Estructura del Proyecto

```
PATI-Confeccion/
├── Backend/                    # API REST en .NET 6
│   ├── PATI.API/              # Controllers y endpoints
│   ├── PATI.Application/      # Servicios y DTOs
│   ├── PATI.Domain/           # Entidades y contratos
│   └── PATI.Infrastructure/   # DbContext y repositorios
├── Frontend/                   # Aplicación React
│   ├── src/
│   │   ├── components/        # Componentes reutilizables
│   │   ├── pages/             # Páginas principales
│   │   ├── contexts/          # Context API
│   │   └── services/          # API services
│   └── public/
├── Database/                   # Scripts SQL
│   ├── 01_CreateDatabase.sql  # Crear BD
│   ├── 02_CreateTables.sql    # Crear tablas
│   ├── 03_InsertTestData.sql  # Datos de prueba
│   ├── 04_TestQueries.sql     # Consultas de prueba
│   ├── ExecuteAll.ps1         # Script automatizado
│   ├── QuickTest.ps1          # Verificación rápida
│   └── README.md              # Guía de ejecución
└── DOCUMENTACION.md           # Documentación técnica
```

## 🚀 Tecnologías

### Backend
- **.NET 6** - Framework principal
- **Entity Framework Core** - ORM
- **SQL Server** - Base de datos
- **ASP.NET Core Identity** - Autenticación
- **JWT** - Tokens de autenticación
- **Swagger** - Documentación API

### Frontend
- **React 18** - Framework UI
- **React Router** - Navegación
- **Axios** - Cliente HTTP
- **Recharts** - Gráficos
- **Context API** - Estado global

### Base de Datos
- **SQL Server 2016+** - Motor de BD
- **19 Tablas principales** - Estructura normalizada
- **Datos de prueba** - Talleres, cortes, asignaciones

## ⚡ Inicio Rápido

### 1. Configurar Base de Datos

```powershell
# Opción A: Script automatizado (Recomendado)
cd Database
.\ExecuteAll.ps1

# Opción B: Scripts individuales
sqlcmd -S localhost -E -i 01_CreateDatabase.sql
sqlcmd -S localhost -E -i 02_CreateTables.sql
sqlcmd -S localhost -E -i 03_InsertTestData.sql

# Verificar instalación
.\QuickTest.ps1
```

### 2. Configurar Backend

```powershell
cd Backend/PATI.API

# Actualizar appsettings.json con tu cadena de conexión
# "Server=localhost;Database=PATI_Confeccion;Trusted_Connection=True;TrustServerCertificate=True;"

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run
```

La API estará disponible en: `http://localhost:5000`  
Swagger UI: `http://localhost:5000/swagger`

### 3. Configurar Frontend

```bash
cd Frontend

# Instalar dependencias
npm install

# Iniciar en modo desarrollo
npm start
```

La aplicación estará en: `http://localhost:3000`

## 🗄️ Base de Datos

### Creación Rápida

```powershell
# En la carpeta Database/
.\ExecuteAll.ps1
```

Este script automáticamente:
1. ✅ Crea la base de datos `PATI_Confeccion`
2. ✅ Crea todas las tablas (19 tablas)
3. ✅ Inserta datos de prueba (5 talleres, 7 cortes, etc.)
4. ✅ Ejecuta consultas de verificación

### Cadenas de Conexión

#### Windows Authentication (Local)
```
Server=localhost;Database=PATI_Confeccion;Trusted_Connection=True;TrustServerCertificate=True;
```

#### SQL Server Express
```
Server=localhost\SQLEXPRESS;Database=PATI_Confeccion;Trusted_Connection=True;TrustServerCertificate=True;
```

#### SQL Authentication
```
Server=localhost;Database=PATI_Confeccion;User Id=sa;Password=TuPassword;TrustServerCertificate=True;
```

### Verificación

```powershell
# Verificar instalación completa
.\QuickTest.ps1

# Ver datos insertados
sqlcmd -S localhost -E -d PATI_Confeccion -Q "SELECT COUNT(*) FROM Talleres"
```

## 📊 Funcionalidades Principales

### Módulos del Sistema

1. **Gestión de Referencias** - Catálogo de prendas
2. **Control de Cortes** - Registro de cortes con colores y tallas
3. **Gestión de Talleres** - Talleres externos
4. **Asignaciones** - Asignar trabajo a talleres
5. **Avances** - Seguimiento de producción
6. **Remisiones** - Control de entregas
7. **Control de Calidad** - Inspección de prendas
8. **Pagos** - Gestión de pagos a talleres
9. **Inventario** - Stock de prendas terminadas
10. **Dashboard** - Métricas y KPIs

## 🔐 Datos de Prueba

### Usuarios (Nota: Los passwords están hasheados, usar Identity para crear usuarios reales)
```
admin@pati.com          - Administrador
supervisor@pati.com     - Supervisor
operador@pati.com       - Operador
```

### Talleres
```
TALL-001 - Confecciones El Progreso
TALL-002 - Taller San José
TALL-003 - Modas y Confecciones Ltda
TALL-004 - Textiles del Norte
TALL-005 - Confecciones Express
```

### Referencias
```
REF-001 - Camiseta Polo Básica
REF-002 - Camisa Manga Larga Ejecutiva
REF-003 - Pantalón Jean Clásico
REF-004 - Blusa Manga Corta Dama
REF-005 - Vestido Casual
```

## 📡 API Endpoints

```
GET    /api/referencias           - Listar referencias
POST   /api/referencias           - Crear referencia
GET    /api/referencias/{id}      - Obtener referencia

GET    /api/talleres              - Listar talleres
POST   /api/talleres              - Crear taller

GET    /api/cortes                - Listar cortes
POST   /api/cortes                - Crear corte

GET    /api/asignaciones          - Listar asignaciones
POST   /api/asignaciones          - Crear asignación

GET    /api/dashboard             - Obtener métricas
```

Ver Swagger para documentación completa: `http://localhost:5000/swagger`

## 🧪 Pruebas

### Probar Base de Datos
```powershell
cd Database
.\QuickTest.ps1
```

### Probar API
```powershell
# Listar talleres
curl http://localhost:5000/api/talleres

# Obtener dashboard
curl http://localhost:5000/api/dashboard
```

### Ejecutar Tests Backend
```bash
cd Backend
dotnet test
```

## 📖 Documentación

- **[DOCUMENTACION.md](DOCUMENTACION.md)** - Documentación técnica completa
- **[Database/README.md](Database/README.md)** - Guía de base de datos
- **[Database/ESTRUCTURA_DATABASE.md](Database/ESTRUCTURA_DATABASE.md)** - Estructura detallada
- **[Database/COMANDOS_PRUEBAS.md](Database/COMANDOS_PRUEBAS.md)** - Comandos útiles

## 🛠️ Comandos Útiles

### Base de Datos

```powershell
# Ejecutar todos los scripts
cd Database
.\ExecuteAll.ps1

# Limpiar datos (mantener estructura)
sqlcmd -S localhost -E -i 05_CleanData.sql

# Reinsertar datos de prueba
sqlcmd -S localhost -E -i 03_InsertTestData.sql

# Verificación rápida
.\QuickTest.ps1
```

### Backend

```bash
# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run

# Ejecutar con watch (auto-reload)
dotnet watch run

# Crear migración (si usa EF Migrations)
dotnet ef migrations add NombreMigracion --project PATI.Infrastructure

# Aplicar migraciones
dotnet ef database update --project PATI.Infrastructure
```

### Frontend

```bash
# Instalar dependencias
npm install

# Iniciar desarrollo
npm start

# Compilar para producción
npm run build

# Ejecutar tests
npm test
```

## 🔧 Troubleshooting

### Base de Datos

**Error: "Cannot open database"**
```powershell
# Verificar que SQL Server está corriendo
sqlcmd -S localhost -E -Q "SELECT @@VERSION"

# Crear la base de datos
sqlcmd -S localhost -E -i Database/01_CreateDatabase.sql
```

**Error: "sqlcmd no reconocido"**
- Instalar [SQL Server Command Line Utilities](https://aka.ms/sqlcmd)

### Backend

**Error de conexión a BD**
- Verificar cadena de conexión en `appsettings.json`
- Verificar que SQL Server está corriendo
- Verificar permisos de usuario

**Puerto 5000 en uso**
```powershell
# Ver qué proceso usa el puerto
netstat -ano | findstr :5000

# Matar el proceso
taskkill /PID <PID> /F
```

### Frontend

**Error de CORS**
- Verificar que el backend está corriendo
- Verificar configuración de CORS en `Program.cs`

## 📋 Requisitos del Sistema

- **Windows 10/11** o **Linux** o **macOS**
- **SQL Server 2016+** (Express, Developer o Enterprise)
- **.NET 6 SDK** - [Descargar](https://dotnet.microsoft.com/download/dotnet/6.0)
- **Node.js 16+** - [Descargar](https://nodejs.org/)
- **SQL Server Management Studio** (opcional) - [Descargar](https://aka.ms/ssmsfullsetup)

## 👥 Contribuir

1. Fork el proyecto
2. Crear rama feature (`git checkout -b feature/NuevaCaracteristica`)
3. Commit cambios (`git commit -m 'Agregar nueva característica'`)
4. Push a la rama (`git push origin feature/NuevaCaracteristica`)
5. Crear Pull Request

## 📄 Licencia

Este proyecto es privado y confidencial.

## 📞 Soporte

Para preguntas o problemas:
- Revisar [DOCUMENTACION.md](DOCUMENTACION.md)
- Revisar [Database/README.md](Database/README.md)
- Consultar [Database/COMANDOS_PRUEBAS.md](Database/COMANDOS_PRUEBAS.md)

---

**Versión:** 1.0  
**Última actualización:** Diciembre 2025
