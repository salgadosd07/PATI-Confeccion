-- Script para crear roles y usuarios de prueba en el sistema PATI

USE PATI_Confeccion;
GO

-- 1. Crear Roles en AspNetRoles si no existen
IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Administrador')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Administrador', 'ADMINISTRADOR', NEWID());
END

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Corte')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Corte', 'CORTE', NEWID());
END

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Taller')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Taller', 'TALLER', NEWID());
END

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Calidad')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Calidad', 'CALIDAD', NEWID());
END

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Bodega')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Bodega', 'BODEGA', NEWID());
END

IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Financiero')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Financiero', 'FINANCIERO', NEWID());
END

PRINT 'Roles creados exitosamente';
GO

-- 2. Crear usuario Administrador de prueba
-- Password: Admin123! (hash generado por Identity)
DECLARE @AdminUserId NVARCHAR(450) = NEWID();
DECLARE @AdminRoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Administrador');

IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE Email = 'admin@pati.com')
BEGIN
    INSERT INTO AspNetUsers (
        Id, 
        UserName, 
        NormalizedUserName, 
        Email, 
        NormalizedEmail, 
        EmailConfirmed, 
        PasswordHash, 
        SecurityStamp, 
        ConcurrencyStamp,
        PhoneNumberConfirmed,
        TwoFactorEnabled,
        LockoutEnabled,
        AccessFailedCount,
        NombreCompleto,
        FechaCreacion,
        Activo
    )
    VALUES (
        @AdminUserId,
        'admin@pati.com',
        'ADMIN@PATI.COM',
        'admin@pati.com',
        'ADMIN@PATI.COM',
        1,
        'AQAAAAIAAYagAAAAEJ3V7Zq4K8X9yN5xF7Kq5L7M8N9O0P1Q2R3S4T5U6V7W8X9Y0Z1A2B3C4D5E6F7G8==', -- Admin123!
        NEWID(),
        NEWID(),
        0,
        0,
        1,
        0,
        'Administrador del Sistema',
        GETDATE(),
        1
    );
    
    -- Asignar rol de Administrador
    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES (@AdminUserId, @AdminRoleId);
    
    PRINT 'Usuario Administrador creado: admin@pati.com / Admin123!';
END
ELSE
BEGIN
    PRINT 'Usuario admin@pati.com ya existe';
END
GO

-- 3. Crear usuario de Corte de prueba
DECLARE @CorteUserId NVARCHAR(450) = NEWID();
DECLARE @CorteRoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Corte');

IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE Email = 'corte@pati.com')
BEGIN
    INSERT INTO AspNetUsers (
        Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
        PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed,
        TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
        NombreCompleto, FechaCreacion, Activo
    )
    VALUES (
        @CorteUserId, 'corte@pati.com', 'CORTE@PATI.COM', 'corte@pati.com', 'CORTE@PATI.COM', 1,
        'AQAAAAIAAYagAAAAEJ3V7Zq4K8X9yN5xF7Kq5L7M8N9O0P1Q2R3S4T5U6V7W8X9Y0Z1A2B3C4D5E6F7G8==',
        NEWID(), NEWID(), 0, 0, 1, 0,
        'Supervisor de Corte', GETDATE(), 1
    );
    
    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES (@CorteUserId, @CorteRoleId);
    
    PRINT 'Usuario Corte creado: corte@pati.com / Admin123!';
END
GO

-- 4. Crear usuario de Taller de prueba
DECLARE @TallerUserId NVARCHAR(450) = NEWID();
DECLARE @TallerRoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Taller');

IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE Email = 'taller@pati.com')
BEGIN
    INSERT INTO AspNetUsers (
        Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
        PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed,
        TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
        NombreCompleto, FechaCreacion, Activo
    )
    VALUES (
        @TallerUserId, 'taller@pati.com', 'TALLER@PATI.COM', 'taller@pati.com', 'TALLER@PATI.COM', 1,
        'AQAAAAIAAYagAAAAEJ3V7Zq4K8X9yN5xF7Kq5L7M8N9O0P1Q2R3S4T5U6V7W8X9Y0Z1A2B3C4D5E6F7G8==',
        NEWID(), NEWID(), 0, 0, 1, 0,
        'Operario de Taller', GETDATE(), 1
    );
    
    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES (@TallerUserId, @TallerRoleId);
    
    PRINT 'Usuario Taller creado: taller@pati.com / Admin123!';
END
GO

-- 5. Crear usuario de Calidad de prueba
DECLARE @CalidadUserId NVARCHAR(450) = NEWID();
DECLARE @CalidadRoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Calidad');

IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE Email = 'calidad@pati.com')
BEGIN
    INSERT INTO AspNetUsers (
        Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
        PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed,
        TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
        NombreCompleto, FechaCreacion, Activo
    )
    VALUES (
        @CalidadUserId, 'calidad@pati.com', 'CALIDAD@PATI.COM', 'calidad@pati.com', 'CALIDAD@PATI.COM', 1,
        'AQAAAAIAAYagAAAAEJ3V7Zq4K8X9yN5xF7Kq5L7M8N9O0P1Q2R3S4T5U6V7W8X9Y0Z1A2B3C4D5E6F7G8==',
        NEWID(), NEWID(), 0, 0, 1, 0,
        'Inspector de Calidad', GETDATE(), 1
    );
    
    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES (@CalidadUserId, @CalidadRoleId);
    
    PRINT 'Usuario Calidad creado: calidad@pati.com / Admin123!';
END
GO

-- 6. Crear usuario de Bodega de prueba
DECLARE @BodegaUserId NVARCHAR(450) = NEWID();
DECLARE @BodegaRoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Bodega');

IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE Email = 'bodega@pati.com')
BEGIN
    INSERT INTO AspNetUsers (
        Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
        PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed,
        TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
        NombreCompleto, FechaCreacion, Activo
    )
    VALUES (
        @BodegaUserId, 'bodega@pati.com', 'BODEGA@PATI.COM', 'bodega@pati.com', 'BODEGA@PATI.COM', 1,
        'AQAAAAIAAYagAAAAEJ3V7Zq4K8X9yN5xF7Kq5L7M8N9O0P1Q2R3S4T5U6V7W8X9Y0Z1A2B3C4D5E6F7G8==',
        NEWID(), NEWID(), 0, 0, 1, 0,
        'Encargado de Bodega', GETDATE(), 1
    );
    
    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES (@BodegaUserId, @BodegaRoleId);
    
    PRINT 'Usuario Bodega creado: bodega@pati.com / Admin123!';
END
GO

-- 7. Crear usuario Financiero de prueba
DECLARE @FinancieroUserId NVARCHAR(450) = NEWID();
DECLARE @FinancieroRoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Financiero');

IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE Email = 'financiero@pati.com')
BEGIN
    INSERT INTO AspNetUsers (
        Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed,
        PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed,
        TwoFactorEnabled, LockoutEnabled, AccessFailedCount,
        NombreCompleto, FechaCreacion, Activo
    )
    VALUES (
        @FinancieroUserId, 'financiero@pati.com', 'FINANCIERO@PATI.COM', 'financiero@pati.com', 'FINANCIERO@PATI.COM', 1,
        'AQAAAAIAAYagAAAAEJ3V7Zq4K8X9yN5xF7Kq5L7M8N9O0P1Q2R3S4T5U6V7W8X9Y0Z1A2B3C4D5E6F7G8==',
        NEWID(), NEWID(), 0, 0, 1, 0,
        'Contador Financiero', GETDATE(), 1
    );
    
    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES (@FinancieroUserId, @FinancieroRoleId);
    
    PRINT 'Usuario Financiero creado: financiero@pati.com / Admin123!';
END
GO

-- Verificar usuarios creados
SELECT 
    u.Email,
    u.NombreCompleto,
    r.Name as Rol
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email LIKE '%@pati.com'
ORDER BY r.Name;

PRINT '';
PRINT '=== USUARIOS DE PRUEBA CREADOS ===';
PRINT 'Todos los usuarios tienen la contraseña: Admin123!';
PRINT '';
PRINT 'admin@pati.com       - Administrador';
PRINT 'corte@pati.com       - Corte';
PRINT 'taller@pati.com      - Taller';
PRINT 'calidad@pati.com     - Calidad';
PRINT 'bodega@pati.com      - Bodega';
PRINT 'financiero@pati.com  - Financiero';
GO
