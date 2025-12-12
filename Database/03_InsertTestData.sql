-- ============================================
-- Inserción de Datos de Prueba - PATI Confección
-- ============================================

USE PATI_Confeccion;
GO

-- ============================================
-- 1. Roles de Usuario
-- ============================================
PRINT 'Insertando roles...';
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES 
    (NEWID(), 'Administrador', 'ADMINISTRADOR', NEWID()),
    (NEWID(), 'Supervisor', 'SUPERVISOR', NEWID()),
    (NEWID(), 'Operador', 'OPERADOR', NEWID()),
    (NEWID(), 'Almacenista', 'ALMACENISTA', NEWID());

GO

-- ============================================
-- 3. Referencias (Tipos de Prenda)
-- ============================================
PRINT 'Insertando referencias...';
INSERT INTO [dbo].[Referencias] ([Codigo], [Nombre], [Descripcion], [TipoPrenda])
VALUES 
    ('REF-001', 'Camiseta Polo Básica', 'Camiseta tipo polo manga corta', 'Polo'),
    ('REF-002', 'Camisa Manga Larga Ejecutiva', 'Camisa formal para hombre', 'Camisa'),
    ('REF-003', 'Pantalón Jean Clásico', 'Pantalón jean corte clásico', 'Pantalón'),
    ('REF-004', 'Blusa Manga Corta Dama', 'Blusa casual para dama', 'Blusa'),
    ('REF-005', 'Vestido Casual', 'Vestido casual tipo A', 'Vestido'),
    ('REF-006', 'Chaqueta Deportiva', 'Chaqueta deportiva unisex', 'Chaqueta'),
    ('REF-007', 'Short Deportivo', 'Short deportivo con bolsillos', 'Short');
GO

-- ============================================
-- 4. Materiales
-- ============================================
PRINT 'Insertando materiales...';
INSERT INTO [dbo].[Materiales] ([Codigo], [Nombre], [Descripcion], [UnidadMedida])
VALUES 
    ('MAT-001', 'Algodón Pima', 'Algodón de alta calidad', 'Metro'),
    ('MAT-002', 'Poliéster', 'Tela sintética resistente', 'Metro'),
    ('MAT-003', 'Mezclilla Strech', 'Tela denim con elastano', 'Metro'),
    ('MAT-004', 'Lino Natural', 'Fibra natural de lino', 'Metro'),
    ('MAT-005', 'Jersey de Algodón', 'Tela de punto elástica', 'Metro'),
    ('MAT-006', 'Popelina', 'Tela de algodón lisa', 'Metro'),
    ('MAT-007', 'Microfibra Deportiva', 'Tela técnica transpirable', 'Metro');
GO

-- ============================================
-- 5. Colores
-- ============================================
PRINT 'Insertando colores...';
INSERT INTO [dbo].[Colores] ([Codigo], [Nombre], [CodigoHex])
VALUES 
    ('COL-001', 'Blanco', '#FFFFFF'),
    ('COL-002', 'Negro', '#000000'),
    ('COL-003', 'Azul Marino', '#000080'),
    ('COL-004', 'Rojo', '#FF0000'),
    ('COL-005', 'Verde', '#008000'),
    ('COL-006', 'Amarillo', '#FFFF00'),
    ('COL-007', 'Gris', '#808080'),
    ('COL-008', 'Beige', '#F5F5DC'),
    ('COL-009', 'Rosa', '#FFC0CB'),
    ('COL-010', 'Azul Claro', '#ADD8E6');
GO

-- ============================================
-- 6. Tallas
-- ============================================
PRINT 'Insertando tallas...';
INSERT INTO [dbo].[Tallas] ([Codigo], [Nombre], [Orden])
VALUES 
    ('XS', 'Extra Small', 1),
    ('S', 'Small', 2),
    ('M', 'Medium', 3),
    ('L', 'Large', 4),
    ('XL', 'Extra Large', 5),
    ('XXL', 'Double Extra Large', 6),
    ('XXXL', 'Triple Extra Large', 7),
    ('28', 'Talla 28', 8),
    ('30', 'Talla 30', 9),
    ('32', 'Talla 32', 10),
    ('34', 'Talla 34', 11),
    ('36', 'Talla 36', 12);
GO

-- ============================================
-- 7. Talleres
-- ============================================
PRINT 'Insertando talleres...';
INSERT INTO [dbo].[Talleres] ([Codigo], [Nombre], [NombreContacto], [Telefono], [Email], [Direccion], [Observaciones])
VALUES 
    ('TALL-001', 'Confecciones El Progreso', 'Pedro Ramírez', '3201234567', 'progreso@email.com', 'Calle 45 #12-34, Bogotá', 'Especializado en camisetas y polos'),
    ('TALL-002', 'Taller San José', 'Ana Martínez', '3107654321', 'sanjose@email.com', 'Carrera 23 #56-78, Medellín', 'Expertos en pantalones'),
    ('TALL-003', 'Modas y Confecciones Ltda', 'Luis Hernández', '3159876543', 'modasyconf@email.com', 'Avenida 68 #34-56, Cali', 'Todo tipo de prendas'),
    ('TALL-004', 'Textiles del Norte', 'Carmen Suárez', '3182345678', 'texnorte@email.com', 'Calle 12 #45-67, Barranquilla', 'Especializado en prendas formales'),
    ('TALL-005', 'Confecciones Express', 'Roberto Silva', '3134567890', 'express@email.com', 'Carrera 7 #89-12, Bucaramanga', 'Entregas rápidas');
GO

-- ============================================
-- 8. Cortes
-- ============================================
PRINT 'Insertando cortes...';
DECLARE @RefPolo INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-001');
DECLARE @RefCamisa INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-002');
DECLARE @RefPantalon INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-003');
DECLARE @RefBlusa INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-004');
DECLARE @RefVestido INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-005');

DECLARE @MatAlgodon INT = (SELECT Id FROM Materiales WHERE Codigo = 'MAT-001');
DECLARE @MatPoliester INT = (SELECT Id FROM Materiales WHERE Codigo = 'MAT-002');
DECLARE @MatMezclilla INT = (SELECT Id FROM Materiales WHERE Codigo = 'MAT-003');
DECLARE @MatJersey INT = (SELECT Id FROM Materiales WHERE Codigo = 'MAT-005');
DECLARE @MatPopelina INT = (SELECT Id FROM Materiales WHERE Codigo = 'MAT-006');

INSERT INTO [dbo].[Cortes] ([CodigoLote], [Mesa], [FechaCorte], [ReferenciaId], [MaterialId], [CantidadTotal], [CantidadProgramada])
VALUES 
    ('LOTE-2024-001', 'Mesa 1', DATEADD(DAY, -15, GETDATE()), @RefPolo, @MatAlgodon, 500, 500),
    ('LOTE-2024-002', 'Mesa 2', DATEADD(DAY, -14, GETDATE()), @RefCamisa, @MatPopelina, 300, 300),
    ('LOTE-2024-003', 'Mesa 1', DATEADD(DAY, -13, GETDATE()), @RefPantalon, @MatMezclilla, 400, 400),
    ('LOTE-2024-004', 'Mesa 3', DATEADD(DAY, -12, GETDATE()), @RefBlusa, @MatJersey, 350, 350),
    ('LOTE-2024-005', 'Mesa 2', DATEADD(DAY, -11, GETDATE()), @RefVestido, @MatAlgodon, 250, 250),
    ('LOTE-2024-006', 'Mesa 1', DATEADD(DAY, -10, GETDATE()), @RefPolo, @MatPoliester, 600, 600),
    ('LOTE-2024-007', 'Mesa 4', DATEADD(DAY, -8, GETDATE()), @RefPantalon, @MatMezclilla, 450, 450);
GO

-- ============================================
-- 9. Corte-Colores
-- ============================================
PRINT 'Insertando corte-colores...';
DECLARE @Corte1 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-001');
DECLARE @Corte2 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-002');
DECLARE @Corte3 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-003');
DECLARE @Corte4 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-004');
DECLARE @Corte5 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-005');

DECLARE @ColorBlanco INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-001');
DECLARE @ColorNegro INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-002');
DECLARE @ColorAzul INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-003');
DECLARE @ColorRojo INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-004');
DECLARE @ColorRosa INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-009');

INSERT INTO [dbo].[CorteColores] ([CorteId], [ColorId], [Cantidad])
VALUES 
    (@Corte1, @ColorBlanco, 200),
    (@Corte1, @ColorNegro, 150),
    (@Corte1, @ColorAzul, 150),
    (@Corte2, @ColorBlanco, 150),
    (@Corte2, @ColorAzul, 150),
    (@Corte3, @ColorAzul, 200),
    (@Corte3, @ColorNegro, 200),
    (@Corte4, @ColorBlanco, 100),
    (@Corte4, @ColorRosa, 150),
    (@Corte4, @ColorRojo, 100),
    (@Corte5, @ColorRojo, 100),
    (@Corte5, @ColorAzul, 150);
GO

-- ============================================
-- 10. Corte-Tallas
-- ============================================
PRINT 'Insertando corte-tallas...';
DECLARE @TallaS INT = (SELECT Id FROM Tallas WHERE Codigo = 'S');
DECLARE @TallaM INT = (SELECT Id FROM Tallas WHERE Codigo = 'M');
DECLARE @TallaL INT = (SELECT Id FROM Tallas WHERE Codigo = 'L');
DECLARE @TallaXL INT = (SELECT Id FROM Tallas WHERE Codigo = 'XL');
DECLARE @Talla30 INT = (SELECT Id FROM Tallas WHERE Codigo = '30');
DECLARE @Talla32 INT = (SELECT Id FROM Tallas WHERE Codigo = '32');
DECLARE @Talla34 INT = (SELECT Id FROM Tallas WHERE Codigo = '34');

DECLARE @Corte1 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-001');
DECLARE @Corte2 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-002');
DECLARE @Corte3 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-003');
DECLARE @Corte4 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-004');
DECLARE @Corte5 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-005');

INSERT INTO [dbo].[CorteTallas] ([CorteId], [TallaId], [Cantidad])
VALUES 
    (@Corte1, @TallaS, 100),
    (@Corte1, @TallaM, 150),
    (@Corte1, @TallaL, 150),
    (@Corte1, @TallaXL, 100),
    (@Corte2, @TallaM, 100),
    (@Corte2, @TallaL, 120),
    (@Corte2, @TallaXL, 80),
    (@Corte3, @Talla30, 100),
    (@Corte3, @Talla32, 150),
    (@Corte3, @Talla34, 150),
    (@Corte4, @TallaS, 120),
    (@Corte4, @TallaM, 150),
    (@Corte4, @TallaL, 80),
    (@Corte5, @TallaS, 80),
    (@Corte5, @TallaM, 100),
    (@Corte5, @TallaL, 70);
GO

-- ============================================
-- 11. Asignaciones a Talleres
-- ============================================
PRINT 'Insertando asignaciones a talleres...';
DECLARE @Taller1 INT = (SELECT Id FROM Talleres WHERE Codigo = 'TALL-001');
DECLARE @Taller2 INT = (SELECT Id FROM Talleres WHERE Codigo = 'TALL-002');
DECLARE @Taller3 INT = (SELECT Id FROM Talleres WHERE Codigo = 'TALL-003');
DECLARE @Taller4 INT = (SELECT Id FROM Talleres WHERE Codigo = 'TALL-004');

DECLARE @RefPolo INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-001');
DECLARE @RefCamisa INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-002');
DECLARE @RefPantalon INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-003');
DECLARE @RefBlusa INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-004');
DECLARE @RefVestido INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-005');

DECLARE @Corte1 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-001');
DECLARE @Corte2 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-002');
DECLARE @Corte3 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-003');
DECLARE @Corte4 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-004');
DECLARE @Corte5 INT = (SELECT Id FROM Cortes WHERE CodigoLote = 'LOTE-2024-005');

INSERT INTO [dbo].[AsignacionesTaller] ([CodigoAsignacion], [TallerId], [ReferenciaId], [CorteId], 
    [FechaAsignacion], [FechaEstimadaEntrega], [CantidadAsignada], [ValorUnitario], [ValorTotal], [Observaciones])
VALUES 
    ('ASIG-2024-001', @Taller1, @RefPolo, @Corte1, DATEADD(DAY, -14, GETDATE()), DATEADD(DAY, 7, GETDATE()), 250, 8500.00, 2125000.00, 'Primera asignación de polos'),
    ('ASIG-2024-002', @Taller1, @RefPolo, @Corte1, DATEADD(DAY, -14, GETDATE()), DATEADD(DAY, 7, GETDATE()), 250, 8500.00, 2125000.00, 'Segunda parte del lote'),
    ('ASIG-2024-003', @Taller4, @RefCamisa, @Corte2, DATEADD(DAY, -13, GETDATE()), DATEADD(DAY, 10, GETDATE()), 300, 12000.00, 3600000.00, 'Camisas ejecutivas'),
    ('ASIG-2024-004', @Taller2, @RefPantalon, @Corte3, DATEADD(DAY, -12, GETDATE()), DATEADD(DAY, 12, GETDATE()), 400, 15000.00, 6000000.00, 'Pantalones jean'),
    ('ASIG-2024-005', @Taller3, @RefBlusa, @Corte4, DATEADD(DAY, -11, GETDATE()), DATEADD(DAY, 9, GETDATE()), 350, 7500.00, 2625000.00, 'Blusas dama'),
    ('ASIG-2024-006', @Taller3, @RefVestido, @Corte5, DATEADD(DAY, -10, GETDATE()), DATEADD(DAY, 14, GETDATE()), 250, 18000.00, 4500000.00, 'Vestidos casuales'),
    ('ASIG-2024-007', @Taller1, @RefPolo, @Corte1, DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, 15, GETDATE()), 200, 8500.00, 1700000.00, 'Reposición de stock');
GO

-- ============================================
-- 12. Avances de Taller
-- ============================================
PRINT 'Insertando avances de taller...';
DECLARE @Asig1 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-001');
DECLARE @Asig2 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-002');
DECLARE @Asig3 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-003');
DECLARE @Asig4 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-004');
DECLARE @Asig5 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-005');

INSERT INTO [dbo].[AvancesTaller] ([AsignacionTallerId], [FechaReporte], [CantidadLista], 
    [CantidadEnProceso], [CantidadPendiente], [CantidadDespachada], [PorcentajeAvance], [Observaciones])
VALUES 
    (@Asig1, DATEADD(DAY, -10, GETDATE()), 100, 100, 50, 0, 40.00, 'Avance inicial - 40%'),
    (@Asig1, DATEADD(DAY, -7, GETDATE()), 180, 50, 20, 0, 72.00, 'Buen ritmo de producción'),
    (@Asig1, DATEADD(DAY, -3, GETDATE()), 230, 20, 0, 200, 92.00, 'Primera entrega realizada'),
    (@Asig2, DATEADD(DAY, -9, GETDATE()), 80, 120, 50, 0, 32.00, 'En proceso'),
    (@Asig2, DATEADD(DAY, -5, GETDATE()), 150, 80, 20, 0, 60.00, 'Avance según lo planeado'),
    (@Asig3, DATEADD(DAY, -8, GETDATE()), 120, 130, 50, 0, 40.00, 'Iniciando producción'),
    (@Asig3, DATEADD(DAY, -4, GETDATE()), 200, 80, 20, 150, 66.67, 'Primera entrega parcial'),
    (@Asig4, DATEADD(DAY, -7, GETDATE()), 150, 150, 100, 0, 37.50, 'Producción normal'),
    (@Asig4, DATEADD(DAY, -3, GETDATE()), 280, 80, 40, 200, 70.00, 'Entrega parcial realizada'),
    (@Asig5, DATEADD(DAY, -6, GETDATE()), 140, 140, 70, 0, 40.00, 'Desarrollo normal');
GO

-- ============================================
-- 13. Remisiones
-- ============================================
PRINT 'Insertando remisiones...';
DECLARE @Asig1 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-001');
DECLARE @Asig3 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-003');
DECLARE @Asig4 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-004');

INSERT INTO [dbo].[Remisiones] ([NumeroRemision], [AsignacionTallerId], [FechaDespacho], [FechaRecepcion],
    [CantidadEnviada], [CantidadRecibida], [RevisadoPor], [EstadoRemision], [Observaciones])
VALUES 
    ('REM-2024-001', @Asig1, DATEADD(DAY, -3, GETDATE()), DATEADD(DAY, -2, GETDATE()), 200, 198, 'María López', 'Recibido', 'Recibido con 2 unidades con defectos'),
    ('REM-2024-002', @Asig3, DATEADD(DAY, -4, GETDATE()), DATEADD(DAY, -3, GETDATE()), 150, 150, 'María López', 'Recibido', 'Recibido conforme'),
    ('REM-2024-003', @Asig4, DATEADD(DAY, -3, GETDATE()), DATEADD(DAY, -2, GETDATE()), 200, 195, 'Carlos García', 'Recibido', 'Algunas imperfecciones menores'),
    ('REM-2024-004', @Asig1, DATEADD(DAY, -1, GETDATE()), NULL, 50, NULL, NULL, 'En Tránsito', 'Pendiente de recepción');
GO

-- ============================================
-- 14. Detalles de Remisiones
-- ============================================
PRINT 'Insertando detalles de remisiones...';
DECLARE @Rem1 INT = (SELECT Id FROM Remisiones WHERE NumeroRemision = 'REM-2024-001');
DECLARE @Rem2 INT = (SELECT Id FROM Remisiones WHERE NumeroRemision = 'REM-2024-002');
DECLARE @Rem3 INT = (SELECT Id FROM Remisiones WHERE NumeroRemision = 'REM-2024-003');

DECLARE @ColorBlanco INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-001');
DECLARE @ColorNegro INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-002');
DECLARE @ColorAzul INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-003');

DECLARE @TallaS INT = (SELECT Id FROM Tallas WHERE Codigo = 'S');
DECLARE @TallaM INT = (SELECT Id FROM Tallas WHERE Codigo = 'M');
DECLARE @TallaL INT = (SELECT Id FROM Tallas WHERE Codigo = 'L');
DECLARE @TallaXL INT = (SELECT Id FROM Tallas WHERE Codigo = 'XL');
DECLARE @Talla30 INT = (SELECT Id FROM Tallas WHERE Codigo = '30');
DECLARE @Talla32 INT = (SELECT Id FROM Tallas WHERE Codigo = '32');
DECLARE @Talla34 INT = (SELECT Id FROM Tallas WHERE Codigo = '34');

INSERT INTO [dbo].[RemisionDetalles] ([RemisionId], [TallaId], [ColorId], [Cantidad])
VALUES 
    (@Rem1, @TallaS, @ColorBlanco, 40),
    (@Rem1, @TallaM, @ColorBlanco, 60),
    (@Rem1, @TallaL, @ColorNegro, 60),
    (@Rem1, @TallaXL, @ColorAzul, 40),
    (@Rem2, @TallaM, @ColorBlanco, 60),
    (@Rem2, @TallaL, @ColorBlanco, 60),
    (@Rem2, @TallaXL, @ColorAzul, 30),
    (@Rem3, @Talla30, @ColorAzul, 50),
    (@Rem3, @Talla32, @ColorAzul, 80),
    (@Rem3, @Talla34, @ColorNegro, 70);
GO

-- ============================================
-- 15. Control de Calidad
-- ============================================
PRINT 'Insertando controles de calidad...';
DECLARE @Rem1 INT = (SELECT Id FROM Remisiones WHERE NumeroRemision = 'REM-2024-001');
DECLARE @Rem2 INT = (SELECT Id FROM Remisiones WHERE NumeroRemision = 'REM-2024-002');
DECLARE @Rem3 INT = (SELECT Id FROM Remisiones WHERE NumeroRemision = 'REM-2024-003');

INSERT INTO [dbo].[ControlesCalidad] ([RemisionId], [FechaControl], [CantidadImperfectos], 
    [CantidadArreglos], [CantidadPendientes], [CantidadAprobados], [CausaImperfecto], 
    [Observaciones], [EstadoArreglos], [RevisadoPor])
VALUES 
    (@Rem1, DATEADD(DAY, -2, GETDATE()), 2, 2, 0, 198, 'Costura defectuosa', 'Dos unidades con problemas de costura en manga', 'En Proceso', 'María López'),
    (@Rem2, DATEADD(DAY, -3, GETDATE()), 0, 0, 0, 150, NULL, 'Todo conforme, excelente calidad', 'N/A', 'María López'),
    (@Rem3, DATEADD(DAY, -2, GETDATE()), 5, 5, 0, 195, 'Manchas y costuras', 'Manchas leves en 3 unidades, costuras en 2', 'Completado', 'Carlos García');
GO

-- ============================================
-- 16. Detalles de Imperfectos
-- ============================================
PRINT 'Insertando detalles de imperfectos...';
DECLARE @Rem1 INT = (SELECT Id FROM Remisiones WHERE NumeroRemision = 'REM-2024-001');
DECLARE @Rem3 INT = (SELECT Id FROM Remisiones WHERE NumeroRemision = 'REM-2024-003');

DECLARE @CC1 INT = (SELECT Id FROM ControlesCalidad WHERE RemisionId = @Rem1);
DECLARE @CC3 INT = (SELECT Id FROM ControlesCalidad WHERE RemisionId = @Rem3);

INSERT INTO [dbo].[DetallesImperfectos] ([ControlCalidadId], [TipoDefecto], [Cantidad], [Descripcion])
VALUES 
    (@CC1, 'Costura defectuosa', 2, 'Costura manga derecha descocida'),
    (@CC3, 'Manchas', 3, 'Manchas leves de aceite en pretina'),
    (@CC3, 'Costura irregular', 2, 'Puntadas irregulares en bota');
GO

-- ============================================
-- 17. Pagos
-- ============================================
PRINT 'Insertando pagos...';
DECLARE @Asig1 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-001');
DECLARE @Asig2 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-002');
DECLARE @Asig3 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-003');
DECLARE @Asig4 INT = (SELECT Id FROM AsignacionesTaller WHERE CodigoAsignacion = 'ASIG-2024-004');

INSERT INTO [dbo].[Pagos] ([NumeroPago], [AsignacionTallerId], [FechaPago], [MontoTotal], 
    [MontoPagado], [EstadoPago], [MetodoPago], [Referencia], [Observaciones], [DiasMora])
VALUES 
    ('PAG-2024-001', @Asig1, DATEADD(DAY, -1, GETDATE()), 2125000.00, 2125000.00, 'Pagado', 'Transferencia', 'TRF-123456', 'Pago completo realizado', 0),
    ('PAG-2024-002', @Asig2, DATEADD(DAY, 5, GETDATE()), 2125000.00, NULL, 'Pendiente', NULL, NULL, 'Pendiente de entrega final', NULL),
    ('PAG-2024-003', @Asig3, DATEADD(DAY, -2, GETDATE()), 3600000.00, 1800000.00, 'Parcial', 'Transferencia', 'TRF-123457', 'Anticipo 50%', 0),
    ('PAG-2024-004', @Asig4, DATEADD(DAY, 8, GETDATE()), 6000000.00, NULL, 'Pendiente', NULL, NULL, 'Pago contra entrega', NULL);
GO

-- ============================================
-- 18. Inventario
-- ============================================
PRINT 'Insertando inventario...';
DECLARE @RefPolo INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-001');
DECLARE @RefCamisa INT = (SELECT Id FROM Referencias WHERE Codigo = 'REF-002');

DECLARE @ColorBlanco INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-001');
DECLARE @ColorNegro INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-002');
DECLARE @ColorAzul INT = (SELECT Id FROM Colores WHERE Codigo = 'COL-003');

DECLARE @TallaS INT = (SELECT Id FROM Tallas WHERE Codigo = 'S');
DECLARE @TallaM INT = (SELECT Id FROM Tallas WHERE Codigo = 'M');
DECLARE @TallaL INT = (SELECT Id FROM Tallas WHERE Codigo = 'L');
DECLARE @TallaXL INT = (SELECT Id FROM Tallas WHERE Codigo = 'XL');

INSERT INTO [dbo].[Inventario] ([ReferenciaId], [TallaId], [ColorId], [CodigoLote], 
    [CantidadDisponible], [CantidadReservada], [FechaIngreso], [Ubicacion], [EstadoInventario])
VALUES 
    (@RefPolo, @TallaS, @ColorBlanco, 'LOTE-2024-001', 40, 0, DATEADD(DAY, -2, GETDATE()), 'Bodega A-1', 'Disponible'),
    (@RefPolo, @TallaM, @ColorBlanco, 'LOTE-2024-001', 60, 10, DATEADD(DAY, -2, GETDATE()), 'Bodega A-1', 'Disponible'),
    (@RefPolo, @TallaL, @ColorNegro, 'LOTE-2024-001', 55, 5, DATEADD(DAY, -2, GETDATE()), 'Bodega A-2', 'Disponible'),
    (@RefPolo, @TallaXL, @ColorAzul, 'LOTE-2024-001', 38, 2, DATEADD(DAY, -2, GETDATE()), 'Bodega A-2', 'Disponible'),
    (@RefCamisa, @TallaM, @ColorBlanco, 'LOTE-2024-002', 58, 2, DATEADD(DAY, -3, GETDATE()), 'Bodega B-1', 'Disponible'),
    (@RefCamisa, @TallaL, @ColorBlanco, 'LOTE-2024-002', 60, 0, DATEADD(DAY, -3, GETDATE()), 'Bodega B-1', 'Disponible'),
    (@RefCamisa, @TallaXL, @ColorAzul, 'LOTE-2024-002', 30, 0, DATEADD(DAY, -3, GETDATE()), 'Bodega B-2', 'Disponible');
GO

-- ============================================
-- 19. Notificaciones
-- ============================================
PRINT 'Insertando notificaciones...';
INSERT INTO [dbo].[Notificaciones] ([Tipo], [Titulo], [Mensaje], [DestinatarioEmail], 
    [DestinatarioTelefono], [FechaEnvio], [Enviada], [Leida], [EntidadRelacionada], [EntidadRelacionadaId])
VALUES 
    ('Entrega', 'Remisión Recibida', 'Se ha recibido la remisión REM-2024-001 con 198 unidades', 'admin@pati.com', '3001234567', DATEADD(DAY, -2, GETDATE()), 1, 1, 'Remision', 1),
    ('Calidad', 'Control de Calidad Realizado', 'Control de calidad completado para REM-2024-001. 2 unidades con defectos', 'supervisor@pati.com', '3007654321', DATEADD(DAY, -2, GETDATE()), 1, 1, 'ControlCalidad', 1),
    ('Pago', 'Pago Realizado', 'Se ha realizado el pago PAG-2024-001 por $2,125,000', 'progreso@email.com', '3201234567', DATEADD(DAY, -1, GETDATE()), 1, 0, 'Pago', 1),
    ('Avance', 'Avance de Producción', 'El taller Confecciones El Progreso reporta 92% de avance', 'admin@pati.com', '3001234567', DATEADD(DAY, -3, GETDATE()), 1, 1, 'AvanceTaller', 3),
    ('Entrega', 'Entrega Pendiente', 'La remisión REM-2024-004 está pendiente de recepción', 'supervisor@pati.com', '3007654321', DATEADD(DAY, -1, GETDATE()), 1, 0, 'Remision', 4),
    ('Alerta', 'Inventario Bajo', 'El inventario de Polo Talla XL Color Azul está bajo', 'almacenista@pati.com', '3009876543', GETDATE(), 1, 0, 'Inventario', 4);
GO

PRINT '==============================================';
PRINT 'DATOS DE PRUEBA INSERTADOS EXITOSAMENTE';
PRINT '==============================================';
PRINT '';
PRINT 'Resumen de registros creados:';
PRINT '- 4 Roles';
PRINT '- 3 Usuarios';
PRINT '- 7 Referencias';
PRINT '- 7 Materiales';
PRINT '- 10 Colores';
PRINT '- 12 Tallas';
PRINT '- 5 Talleres';
PRINT '- 7 Cortes';
PRINT '- Múltiples Corte-Colores y Corte-Tallas';
PRINT '- 7 Asignaciones a Talleres';
PRINT '- 10 Avances de Taller';
PRINT '- 4 Remisiones';
PRINT '- Detalles de Remisiones';
PRINT '- 3 Controles de Calidad';
PRINT '- Detalles de Imperfectos';
PRINT '- 4 Pagos';
PRINT '- 7 Registros de Inventario';
PRINT '- 6 Notificaciones';
PRINT '';
GO
