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
