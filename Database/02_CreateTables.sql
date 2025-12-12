-- ============================================
-- Creación de Tablas - PATI Confección
-- ============================================

USE PATI_Confeccion;
GO

-- ============================================
-- Tablas de Identity (ASP.NET Core Identity)
-- ============================================

-- Tabla de Roles
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetRoles] (
        [Id] NVARCHAR(450) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(256) NULL,
        [NormalizedName] NVARCHAR(256) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL
    );
    CREATE UNIQUE INDEX [RoleNameIndex] ON [dbo].[AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
    PRINT 'Tabla AspNetRoles creada.';
END
GO

-- Tabla de Usuarios
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUsers] (
        [Id] NVARCHAR(450) NOT NULL PRIMARY KEY,
        [UserName] NVARCHAR(256) NULL,
        [NormalizedUserName] NVARCHAR(256) NULL,
        [Email] NVARCHAR(256) NULL,
        [NormalizedEmail] NVARCHAR(256) NULL,
        [EmailConfirmed] BIT NOT NULL,
        [PasswordHash] NVARCHAR(MAX) NULL,
        [SecurityStamp] NVARCHAR(MAX) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL,
        [PhoneNumber] NVARCHAR(MAX) NULL,
        [PhoneNumberConfirmed] BIT NOT NULL,
        [TwoFactorEnabled] BIT NOT NULL,
        [LockoutEnd] DATETIMEOFFSET(7) NULL,
        [LockoutEnabled] BIT NOT NULL,
        [AccessFailedCount] INT NOT NULL,
        -- Campos personalizados
        [NombreCompleto] NVARCHAR(200) NOT NULL DEFAULT '',
        [FechaCreacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1
    );
    CREATE UNIQUE INDEX [UserNameIndex] ON [dbo].[AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
    CREATE INDEX [EmailIndex] ON [dbo].[AspNetUsers] ([NormalizedEmail]);
    PRINT 'Tabla AspNetUsers creada.';
END
GO

-- Tabla de Claims de Usuarios
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserClaims] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [UserId] NVARCHAR(450) NOT NULL,
        [ClaimType] NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims] ([UserId]);
    PRINT 'Tabla AspNetUserClaims creada.';
END
GO

-- Tabla de Logins de Usuarios
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserLogins] (
        [LoginProvider] NVARCHAR(450) NOT NULL,
        [ProviderKey] NVARCHAR(450) NOT NULL,
        [ProviderDisplayName] NVARCHAR(MAX) NULL,
        [UserId] NVARCHAR(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins] ([UserId]);
    PRINT 'Tabla AspNetUserLogins creada.';
END
GO

-- Tabla de Tokens de Usuarios
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserTokens] (
        [UserId] NVARCHAR(450) NOT NULL,
        [LoginProvider] NVARCHAR(450) NOT NULL,
        [Name] NVARCHAR(450) NOT NULL,
        [Value] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    PRINT 'Tabla AspNetUserTokens creada.';
END
GO

-- Tabla de Relación Usuario-Rol
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserRoles] (
        [UserId] NVARCHAR(450) NOT NULL,
        [RoleId] NVARCHAR(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles] ([RoleId]);
    PRINT 'Tabla AspNetUserRoles creada.';
END
GO

-- Tabla de Claims de Roles
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetRoleClaims] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RoleId] NVARCHAR(450) NOT NULL,
        [ClaimType] NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims] ([RoleId]);
    PRINT 'Tabla AspNetRoleClaims creada.';
END
GO

-- ============================================
-- Tablas Maestras
-- ============================================

-- Tabla de Referencias (Tipos de prenda)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Referencias]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Referencias] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Codigo] NVARCHAR(50) NOT NULL UNIQUE,
        [Nombre] NVARCHAR(200) NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [TipoPrenda] NVARCHAR(100) NOT NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1
    );
    CREATE INDEX [IX_Referencias_Codigo] ON [dbo].[Referencias] ([Codigo]);
    PRINT 'Tabla Referencias creada.';
END
GO

-- Tabla de Materiales
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Materiales]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Materiales] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Codigo] NVARCHAR(50) NOT NULL UNIQUE,
        [Nombre] NVARCHAR(200) NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [UnidadMedida] NVARCHAR(50) NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1
    );
    CREATE INDEX [IX_Materiales_Codigo] ON [dbo].[Materiales] ([Codigo]);
    PRINT 'Tabla Materiales creada.';
END
GO

-- Tabla de Colores
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Colores]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Colores] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Codigo] NVARCHAR(50) NOT NULL UNIQUE,
        [Nombre] NVARCHAR(100) NOT NULL,
        [CodigoHex] NVARCHAR(7) NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1
    );
    CREATE INDEX [IX_Colores_Codigo] ON [dbo].[Colores] ([Codigo]);
    PRINT 'Tabla Colores creada.';
END
GO

-- Tabla de Tallas
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tallas]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Tallas] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Codigo] NVARCHAR(50) NOT NULL UNIQUE,
        [Nombre] NVARCHAR(100) NOT NULL,
        [Orden] INT NOT NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1
    );
    CREATE INDEX [IX_Tallas_Codigo] ON [dbo].[Tallas] ([Codigo]);
    CREATE INDEX [IX_Tallas_Orden] ON [dbo].[Tallas] ([Orden]);
    PRINT 'Tabla Tallas creada.';
END
GO

-- Tabla de Talleres
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Talleres]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Talleres] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Codigo] NVARCHAR(50) NOT NULL UNIQUE,
        [Nombre] NVARCHAR(200) NOT NULL,
        [NombreContacto] NVARCHAR(200) NULL,
        [Telefono] NVARCHAR(20) NULL,
        [Email] NVARCHAR(200) NULL,
        [Direccion] NVARCHAR(500) NULL,
        [Observaciones] NVARCHAR(1000) NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1
    );
    CREATE INDEX [IX_Talleres_Codigo] ON [dbo].[Talleres] ([Codigo]);
    PRINT 'Tabla Talleres creada.';
END
GO

-- ============================================
-- Tablas de Operaciones
-- ============================================

-- Tabla de Cortes
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cortes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Cortes] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [CodigoLote] NVARCHAR(100) NOT NULL UNIQUE,
        [Mesa] NVARCHAR(50) NOT NULL,
        [FechaCorte] DATETIME2 NOT NULL,
        [ReferenciaId] INT NOT NULL,
        [MaterialId] INT NOT NULL,
        [CantidadTotal] INT NOT NULL,
        [CantidadProgramada] INT NOT NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [FK_Cortes_Referencias] FOREIGN KEY ([ReferenciaId]) REFERENCES [dbo].[Referencias] ([Id]),
        CONSTRAINT [FK_Cortes_Materiales] FOREIGN KEY ([MaterialId]) REFERENCES [dbo].[Materiales] ([Id])
    );
    CREATE INDEX [IX_Cortes_CodigoLote] ON [dbo].[Cortes] ([CodigoLote]);
    CREATE INDEX [IX_Cortes_ReferenciaId] ON [dbo].[Cortes] ([ReferenciaId]);
    CREATE INDEX [IX_Cortes_FechaCorte] ON [dbo].[Cortes] ([FechaCorte]);
    PRINT 'Tabla Cortes creada.';
END
GO

-- Tabla de Corte-Color (Relación muchos a muchos)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CorteColores]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[CorteColores] (
        [CorteId] INT NOT NULL,
        [ColorId] INT NOT NULL,
        [Cantidad] INT NOT NULL,
        CONSTRAINT [PK_CorteColores] PRIMARY KEY ([CorteId], [ColorId]),
        CONSTRAINT [FK_CorteColores_Cortes] FOREIGN KEY ([CorteId]) REFERENCES [dbo].[Cortes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CorteColores_Colores] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Colores] ([Id])
    );
    PRINT 'Tabla CorteColores creada.';
END
GO

-- Tabla de Corte-Talla (Relación muchos a muchos)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CorteTallas]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[CorteTallas] (
        [CorteId] INT NOT NULL,
        [TallaId] INT NOT NULL,
        [Cantidad] INT NOT NULL,
        CONSTRAINT [PK_CorteTallas] PRIMARY KEY ([CorteId], [TallaId]),
        CONSTRAINT [FK_CorteTallas_Cortes] FOREIGN KEY ([CorteId]) REFERENCES [dbo].[Cortes] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CorteTallas_Tallas] FOREIGN KEY ([TallaId]) REFERENCES [dbo].[Tallas] ([Id])
    );
    PRINT 'Tabla CorteTallas creada.';
END
GO

-- Tabla de Asignaciones a Talleres
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AsignacionesTaller]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AsignacionesTaller] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [CodigoAsignacion] NVARCHAR(100) NOT NULL UNIQUE,
        [TallerId] INT NOT NULL,
        [ReferenciaId] INT NOT NULL,
        [CorteId] INT NOT NULL,
        [FechaAsignacion] DATETIME2 NOT NULL,
        [FechaEstimadaEntrega] DATETIME2 NULL,
        [CantidadAsignada] INT NOT NULL,
        [ValorUnitario] DECIMAL(18,2) NULL,
        [ValorTotal] DECIMAL(18,2) NULL,
        [Observaciones] NVARCHAR(1000) NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [FK_AsignacionesTaller_Talleres] FOREIGN KEY ([TallerId]) REFERENCES [dbo].[Talleres] ([Id]),
        CONSTRAINT [FK_AsignacionesTaller_Referencias] FOREIGN KEY ([ReferenciaId]) REFERENCES [dbo].[Referencias] ([Id]),
        CONSTRAINT [FK_AsignacionesTaller_Cortes] FOREIGN KEY ([CorteId]) REFERENCES [dbo].[Cortes] ([Id])
    );
    CREATE INDEX [IX_AsignacionesTaller_CodigoAsignacion] ON [dbo].[AsignacionesTaller] ([CodigoAsignacion]);
    CREATE INDEX [IX_AsignacionesTaller_TallerId] ON [dbo].[AsignacionesTaller] ([TallerId]);
    CREATE INDEX [IX_AsignacionesTaller_FechaAsignacion] ON [dbo].[AsignacionesTaller] ([FechaAsignacion]);
    PRINT 'Tabla AsignacionesTaller creada.';
END
GO

-- Tabla de Avances de Taller
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AvancesTaller]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AvancesTaller] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [AsignacionTallerId] INT NOT NULL,
        [FechaReporte] DATETIME2 NOT NULL,
        [CantidadLista] INT NOT NULL,
        [CantidadEnProceso] INT NOT NULL,
        [CantidadPendiente] INT NOT NULL,
        [CantidadDespachada] INT NOT NULL,
        [PorcentajeAvance] DECIMAL(5,2) NOT NULL,
        [Observaciones] NVARCHAR(1000) NULL,
        [UrlFotoEvidencia] NVARCHAR(500) NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [FK_AvancesTaller_AsignacionesTaller] FOREIGN KEY ([AsignacionTallerId]) REFERENCES [dbo].[AsignacionesTaller] ([Id])
    );
    CREATE INDEX [IX_AvancesTaller_AsignacionTallerId] ON [dbo].[AvancesTaller] ([AsignacionTallerId]);
    CREATE INDEX [IX_AvancesTaller_FechaReporte] ON [dbo].[AvancesTaller] ([FechaReporte]);
    PRINT 'Tabla AvancesTaller creada.';
END
GO

-- Tabla de Remisiones
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Remisiones]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Remisiones] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [NumeroRemision] NVARCHAR(100) NOT NULL UNIQUE,
        [AsignacionTallerId] INT NOT NULL,
        [FechaDespacho] DATETIME2 NOT NULL,
        [FechaRecepcion] DATETIME2 NULL,
        [CantidadEnviada] INT NOT NULL,
        [CantidadRecibida] INT NULL,
        [RevisadoPor] NVARCHAR(200) NULL,
        [EstadoRemision] NVARCHAR(50) NOT NULL DEFAULT 'Pendiente',
        [Observaciones] NVARCHAR(1000) NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [FK_Remisiones_AsignacionesTaller] FOREIGN KEY ([AsignacionTallerId]) REFERENCES [dbo].[AsignacionesTaller] ([Id])
    );
    CREATE INDEX [IX_Remisiones_NumeroRemision] ON [dbo].[Remisiones] ([NumeroRemision]);
    CREATE INDEX [IX_Remisiones_AsignacionTallerId] ON [dbo].[Remisiones] ([AsignacionTallerId]);
    CREATE INDEX [IX_Remisiones_EstadoRemision] ON [dbo].[Remisiones] ([EstadoRemision]);
    PRINT 'Tabla Remisiones creada.';
END
GO

-- Tabla de Detalle de Remisiones
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RemisionDetalles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[RemisionDetalles] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RemisionId] INT NOT NULL,
        [TallaId] INT NOT NULL,
        [ColorId] INT NOT NULL,
        [Cantidad] INT NOT NULL,
        CONSTRAINT [FK_RemisionDetalles_Remisiones] FOREIGN KEY ([RemisionId]) REFERENCES [dbo].[Remisiones] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RemisionDetalles_Tallas] FOREIGN KEY ([TallaId]) REFERENCES [dbo].[Tallas] ([Id]),
        CONSTRAINT [FK_RemisionDetalles_Colores] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Colores] ([Id])
    );
    CREATE INDEX [IX_RemisionDetalles_RemisionId] ON [dbo].[RemisionDetalles] ([RemisionId]);
    PRINT 'Tabla RemisionDetalles creada.';
END
GO

-- Tabla de Control de Calidad
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ControlesCalidad]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ControlesCalidad] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RemisionId] INT NOT NULL,
        [FechaControl] DATETIME2 NOT NULL,
        [CantidadImperfectos] INT NOT NULL,
        [CantidadArreglos] INT NOT NULL,
        [CantidadPendientes] INT NOT NULL,
        [CantidadAprobados] INT NOT NULL,
        [CausaImperfecto] NVARCHAR(500) NULL,
        [Observaciones] NVARCHAR(1000) NULL,
        [EstadoArreglos] NVARCHAR(50) NOT NULL DEFAULT 'Pendiente',
        [RevisadoPor] NVARCHAR(200) NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [FK_ControlesCalidad_Remisiones] FOREIGN KEY ([RemisionId]) REFERENCES [dbo].[Remisiones] ([Id])
    );
    CREATE INDEX [IX_ControlesCalidad_RemisionId] ON [dbo].[ControlesCalidad] ([RemisionId]);
    CREATE INDEX [IX_ControlesCalidad_FechaControl] ON [dbo].[ControlesCalidad] ([FechaControl]);
    PRINT 'Tabla ControlesCalidad creada.';
END
GO

-- Tabla de Detalle de Imperfectos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetallesImperfectos]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DetallesImperfectos] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [ControlCalidadId] INT NOT NULL,
        [TipoDefecto] NVARCHAR(200) NOT NULL,
        [Cantidad] INT NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        CONSTRAINT [FK_DetallesImperfectos_ControlesCalidad] FOREIGN KEY ([ControlCalidadId]) REFERENCES [dbo].[ControlesCalidad] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_DetallesImperfectos_ControlCalidadId] ON [dbo].[DetallesImperfectos] ([ControlCalidadId]);
    PRINT 'Tabla DetallesImperfectos creada.';
END
GO

-- Tabla de Pagos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pagos]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Pagos] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [NumeroPago] NVARCHAR(100) NOT NULL UNIQUE,
        [AsignacionTallerId] INT NOT NULL,
        [FechaPago] DATETIME2 NOT NULL,
        [MontoTotal] DECIMAL(18,2) NOT NULL,
        [MontoPagado] DECIMAL(18,2) NULL,
        [EstadoPago] NVARCHAR(50) NOT NULL DEFAULT 'Pendiente',
        [MetodoPago] NVARCHAR(100) NULL,
        [Referencia] NVARCHAR(200) NULL,
        [Observaciones] NVARCHAR(1000) NULL,
        [DiasMora] INT NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [FK_Pagos_AsignacionesTaller] FOREIGN KEY ([AsignacionTallerId]) REFERENCES [dbo].[AsignacionesTaller] ([Id])
    );
    CREATE INDEX [IX_Pagos_NumeroPago] ON [dbo].[Pagos] ([NumeroPago]);
    CREATE INDEX [IX_Pagos_AsignacionTallerId] ON [dbo].[Pagos] ([AsignacionTallerId]);
    CREATE INDEX [IX_Pagos_EstadoPago] ON [dbo].[Pagos] ([EstadoPago]);
    PRINT 'Tabla Pagos creada.';
END
GO

-- Tabla de Inventario
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Inventario]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Inventario] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [ReferenciaId] INT NOT NULL,
        [TallaId] INT NOT NULL,
        [ColorId] INT NOT NULL,
        [CodigoLote] NVARCHAR(100) NULL,
        [CantidadDisponible] INT NOT NULL,
        [CantidadReservada] INT NOT NULL,
        [FechaIngreso] DATETIME2 NULL,
        [Ubicacion] NVARCHAR(200) NULL,
        [EstadoInventario] NVARCHAR(50) NOT NULL DEFAULT 'Disponible',
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [FK_Inventario_Referencias] FOREIGN KEY ([ReferenciaId]) REFERENCES [dbo].[Referencias] ([Id]),
        CONSTRAINT [FK_Inventario_Tallas] FOREIGN KEY ([TallaId]) REFERENCES [dbo].[Tallas] ([Id]),
        CONSTRAINT [FK_Inventario_Colores] FOREIGN KEY ([ColorId]) REFERENCES [dbo].[Colores] ([Id])
    );
    CREATE INDEX [IX_Inventario_ReferenciaId] ON [dbo].[Inventario] ([ReferenciaId]);
    CREATE INDEX [IX_Inventario_EstadoInventario] ON [dbo].[Inventario] ([EstadoInventario]);
    PRINT 'Tabla Inventario creada.';
END
GO

-- Tabla de Notificaciones
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notificaciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Notificaciones] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Tipo] NVARCHAR(50) NOT NULL,
        [Titulo] NVARCHAR(200) NOT NULL,
        [Mensaje] NVARCHAR(1000) NOT NULL,
        [DestinatarioId] NVARCHAR(450) NULL,
        [DestinatarioEmail] NVARCHAR(200) NULL,
        [DestinatarioTelefono] NVARCHAR(20) NULL,
        [FechaEnvio] DATETIME2 NOT NULL,
        [Enviada] BIT NOT NULL DEFAULT 0,
        [Leida] BIT NOT NULL DEFAULT 0,
        [EntidadRelacionada] NVARCHAR(100) NULL,
        [EntidadRelacionadaId] INT NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [FechaModificacion] DATETIME2 NULL,
        [Activo] BIT NOT NULL DEFAULT 1
    );
    CREATE INDEX [IX_Notificaciones_DestinatarioId] ON [dbo].[Notificaciones] ([DestinatarioId]);
    CREATE INDEX [IX_Notificaciones_FechaEnvio] ON [dbo].[Notificaciones] ([FechaEnvio]);
    CREATE INDEX [IX_Notificaciones_Tipo] ON [dbo].[Notificaciones] ([Tipo]);
    PRINT 'Tabla Notificaciones creada.';
END
GO

PRINT 'Todas las tablas han sido creadas exitosamente.';
GO
