-- ============================================
-- Script de Limpieza - PATI Confección
-- Usa este script para limpiar todos los datos
-- pero mantener la estructura de tablas
-- ============================================

USE PATI_Confeccion;
GO

PRINT '==============================================';
PRINT 'Iniciando limpieza de datos...';
PRINT '==============================================';
PRINT '';

-- Deshabilitar constraints temporalmente
PRINT 'Deshabilitando constraints...';
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- Limpiar datos en orden inverso a las dependencias
PRINT 'Limpiando tablas de detalle...';
DELETE FROM DetallesImperfectos;
DELETE FROM RemisionDetalles;
DELETE FROM CorteColores;
DELETE FROM CorteTallas;
PRINT 'Tablas de detalle limpiadas.';
GO

PRINT 'Limpiando tablas transaccionales...';
DELETE FROM Notificaciones;
DELETE FROM Inventario;
DELETE FROM Pagos;
DELETE FROM ControlesCalidad;
DELETE FROM Remisiones;
DELETE FROM AvancesTaller;
DELETE FROM AsignacionesTaller;
DELETE FROM Cortes;
PRINT 'Tablas transaccionales limpiadas.';
GO

PRINT 'Limpiando tablas maestras...';
DELETE FROM Talleres;
DELETE FROM Tallas;
DELETE FROM Colores;
DELETE FROM Materiales;
DELETE FROM Referencias;
PRINT 'Tablas maestras limpiadas.';
GO

PRINT 'Limpiando tablas de Identity...';
DELETE FROM AspNetUserRoles;
DELETE FROM AspNetUserClaims;
DELETE FROM AspNetUserLogins;
DELETE FROM AspNetUserTokens;
DELETE FROM AspNetRoleClaims;
DELETE FROM AspNetUsers;
DELETE FROM AspNetRoles;
PRINT 'Tablas de Identity limpiadas.';
GO

-- Habilitar constraints nuevamente
PRINT 'Habilitando constraints...';
EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL';
GO

-- Resetear los identity seeds a 0
PRINT 'Reseteando identity seeds...';
DBCC CHECKIDENT ('Referencias', RESEED, 0);
DBCC CHECKIDENT ('Materiales', RESEED, 0);
DBCC CHECKIDENT ('Colores', RESEED, 0);
DBCC CHECKIDENT ('Tallas', RESEED, 0);
DBCC CHECKIDENT ('Talleres', RESEED, 0);
DBCC CHECKIDENT ('Cortes', RESEED, 0);
DBCC CHECKIDENT ('AsignacionesTaller', RESEED, 0);
DBCC CHECKIDENT ('AvancesTaller', RESEED, 0);
DBCC CHECKIDENT ('Remisiones', RESEED, 0);
DBCC CHECKIDENT ('RemisionDetalles', RESEED, 0);
DBCC CHECKIDENT ('ControlesCalidad', RESEED, 0);
DBCC CHECKIDENT ('DetallesImperfectos', RESEED, 0);
DBCC CHECKIDENT ('Pagos', RESEED, 0);
DBCC CHECKIDENT ('Inventario', RESEED, 0);
DBCC CHECKIDENT ('Notificaciones', RESEED, 0);
DBCC CHECKIDENT ('AspNetUserClaims', RESEED, 0);
DBCC CHECKIDENT ('AspNetRoleClaims', RESEED, 0);
GO

PRINT '';
PRINT '==============================================';
PRINT 'Limpieza completada exitosamente.';
PRINT 'Todas las tablas están vacías.';
PRINT '==============================================';
GO
