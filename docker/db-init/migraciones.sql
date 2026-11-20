-- Cambios de esquema que se aplican SOLOS sobre una base que ya existe: init-db.sh corre este
-- archivo en cada "docker compose up" (o sea, en cada ventsoft-actualizar), después de crear la
-- base si hacía falta. Cada cambio va protegido con un IF para que correrlo de nuevo no haga
-- nada — y sobre una base recién creada tampoco, porque SQLQueryVentSoft.sql ya trae la columna.
--
-- Al agregar una columna/tabla nueva: ponerla en SQLQueryVentSoft.sql (bases nuevas) Y acá
-- (bases que ya existen), siempre con su IF.

-- 16/11/2026: código de barras de la caja, distinto del de la unidad (ver Articulo.CodigoPaquete).
IF COL_LENGTH('Articulo', 'CodigoPaquete') IS NULL
    ALTER TABLE Articulo ADD CodigoPaquete varchar(40) NULL;
GO

-- 19/11/2026: promociones "N por $X" (ej. Chicle 2x500), ver Promocion.CantidadPromo.
IF COL_LENGTH('Promocion', 'CantidadPromo') IS NULL
    ALTER TABLE Promocion ADD CantidadPromo int NULL;
GO
IF COL_LENGTH('Promocion', 'PrecioPromo') IS NULL
    ALTER TABLE Promocion ADD PrecioPromo money NULL;
GO
IF COL_LENGTH('Promocion', 'EsPrecioPorMayor') IS NULL
    ALTER TABLE Promocion ADD EsPrecioPorMayor bit NOT NULL DEFAULT 0;
GO

-- 20/11/2026: deudas de fracciones de centavo (ej. 1000 / 12 por paquete) que mostraban
-- "Debe $ 0,00" en compras/ventas pagadas completas. Desde ahora se redondea al registrar;
-- esto limpia las que ya estaban. Correrlo de nuevo no cambia nada.
UPDATE Compra SET PorPagar = 0 WHERE PorPagar <> 0 AND ABS(PorPagar) < 0.01;
UPDATE Venta SET PorPagar = 0 WHERE PorPagar <> 0 AND ABS(PorPagar) < 0.01;
GO

-- 20/11/2026: anular ventas y compras (Estado = "AN"), con el motivo. Ver AnularVentaCommand.
IF COL_LENGTH('Venta', 'MotivoAnulacion') IS NULL
    ALTER TABLE Venta ADD MotivoAnulacion varchar(200) NULL;
GO
IF COL_LENGTH('Compra', 'MotivoAnulacion') IS NULL
    ALTER TABLE Compra ADD MotivoAnulacion varchar(200) NULL;
GO
