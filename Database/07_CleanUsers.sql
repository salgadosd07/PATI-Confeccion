-- Script para limpiar usuarios antiguos y permitir crear nuevos

USE PATI_Confeccion;
GO

-- Eliminar relaciones de roles de usuarios
DELETE FROM AspNetUserRoles 
WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE Email LIKE '%@pati.com');

-- Eliminar usuarios
DELETE FROM AspNetUsers 
WHERE Email LIKE '%@pati.com';

PRINT 'Usuarios eliminados. Ahora puedes crear nuevos usuarios con register-users.html';
GO

SELECT COUNT(*) as UsuariosRestantes FROM AspNetUsers;
GO
