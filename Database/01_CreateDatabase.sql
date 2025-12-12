-- ============================================
-- Script de Creación de Base de Datos PATI
-- Sistema de Gestión de Confección
-- ============================================

-- Crear la base de datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PATI_Confeccion')
BEGIN
    CREATE DATABASE PATI_Confeccion;
    PRINT 'Base de datos PATI_Confeccion creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos PATI_Confeccion ya existe.';
END
GO

USE PATI_Confeccion;
GO

PRINT 'Base de datos PATI_Confeccion seleccionada.';
GO
