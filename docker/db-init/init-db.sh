#!/bin/bash
# Corre UNA sola vez cuando se levanta docker-compose por primera vez (o cuando el
# volumen de datos de SQL Server está vacío): crea la base VentSoft desde
# SQLQueryVentSoft.sql si todavía no existe, y asegura el login/usuario dedicado que
# usa la API (no "sa") con permisos de lectura/escritura sobre esa base nomás.
#
# Es seguro correrlo de nuevo en cada "docker compose up": si la base ya existe no la
# vuelve a crear (el script SQLQueryVentSoft.sql no tiene guards IF NOT EXISTS, así que
# correrlo dos veces rompería), y el login/usuario se crean con IF NOT EXISTS.
set -euo pipefail

# La imagen de SQL Server 2022 trae las herramientas en mssql-tools18 (conexión
# cifrada por defecto); versiones viejas las traían en mssql-tools. Se prueban los
# dos por si el día de mañana se cambia el tag de imagen en docker-compose.yml.
SQLCMD="/opt/mssql-tools18/bin/sqlcmd"
[ -x "$SQLCMD" ] || SQLCMD="/opt/mssql-tools/bin/sqlcmd"

sql() {
  "$SQLCMD" -S sqlserver -U sa -P "$MSSQL_SA_PASSWORD" -C "$@"
}

echo "[db-init] Esperando a que SQL Server acepte conexiones..."
until sql -Q "SELECT 1" > /dev/null 2>&1; do
  sleep 2
done
echo "[db-init] SQL Server listo."

EXISTE=$(sql -h -1 -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name = 'VentSoft'" | tr -d '[:space:]')

if [ "$EXISTE" = "0" ]; then
  echo "[db-init] La base VentSoft no existe todavía: corriendo SQLQueryVentSoft.sql..."
  sql -i /scripts/SQLQueryVentSoft.sql
  echo "[db-init] Base creada."
else
  echo "[db-init] La base VentSoft ya existe, no se vuelve a crear (los datos se conservan)."
fi

echo "[db-init] Asegurando el login '$APP_DB_USER' para la API..."
sql -Q "
IF NOT EXISTS (SELECT 1 FROM sys.sql_logins WHERE name = '$APP_DB_USER')
BEGIN
    CREATE LOGIN [$APP_DB_USER] WITH PASSWORD = '$APP_DB_PASSWORD', CHECK_POLICY = OFF;
END
ELSE
BEGIN
    ALTER LOGIN [$APP_DB_USER] WITH PASSWORD = '$APP_DB_PASSWORD';
END
"

echo "[db-init] Asegurando el usuario de base de datos y sus permisos..."
sql -d VentSoft -Q "
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = '$APP_DB_USER')
BEGIN
    CREATE USER [$APP_DB_USER] FOR LOGIN [$APP_DB_USER];
    ALTER ROLE db_datareader ADD MEMBER [$APP_DB_USER];
    ALTER ROLE db_datawriter ADD MEMBER [$APP_DB_USER];
END
"

echo "[db-init] Listo."
