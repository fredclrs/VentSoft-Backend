#!/bin/bash
# Instala VentSoft completo en un servidor Ubuntu NUEVO (una instalación por negocio).
# Se corre como root, en el servidor (se descarga primero y después se ejecuta, en vez
# de "curl | bash", para que ningún comando de adentro se coma el resto del script):
#
#   curl -fsSL https://raw.githubusercontent.com/fredclrs/VentSoft-Backend/master/scripts/instalar-servidor.sh -o instalar.sh && bash instalar.sh
#
# Opcional: pasar el dominio si ya lo tenés (ej. DuckDNS); si no, usa <ip>.sslip.io:
#
#   bash instalar.sh mialmacen.duckdns.org
#
# Qué hace (y es seguro volver a correrlo: lo que ya está hecho lo saltea):
#   1. Memoria de reserva (swap) de 2 GB si el servidor no tiene.
#   2. Instala Docker y git si faltan.
#   3. Clona los 2 repos uno al lado del otro en /root (o hace git pull si ya están).
#   4. Crea el .env con contraseñas aleatorias nuevas (solo si no existe: nunca pisa
#      uno existente, porque cambiar las claves rompería la base/sesiones/datos cifrados).
#   5. Firewall: solo deja entrar SSH (22) y web (80/443).
#   6. Levanta todo con docker compose.
#   7. Copia nocturna de la base (.bak) en /root/ventsoft-backups, guarda las últimas 7.
#   8. Comandos cortos para administrar: ventsoft-estado, ventsoft-reiniciar,
#      ventsoft-actualizar, ventsoft-backup.
set -euo pipefail

DOMINIO="${1:-}"
DIR_BACK=/root/VentSoft-Backend
DIR_FRONT=/root/VentSoft-FrontEnd
DIR_COPIAS=/root/ventsoft-backups

paso() { echo; echo "==> $*"; }

if [ "$(id -u)" != "0" ]; then
  echo "Este script se corre como root." >&2
  exit 1
fi

export DEBIAN_FRONTEND=noninteractive

paso "1/8 Memoria de reserva (swap)"
if [ -z "$(swapon --show)" ]; then
  fallocate -l 2G /swapfile
  chmod 600 /swapfile
  mkswap /swapfile > /dev/null
  swapon /swapfile
  grep -q '^/swapfile' /etc/fstab || echo '/swapfile none swap sw 0 0' >> /etc/fstab
  echo "Swap de 2 GB creado."
else
  echo "Ya hay swap, no se toca."
fi

paso "2/8 Docker y git"
apt-get update -qq
apt-get install -y -qq git curl ufw cron > /dev/null
if ! command -v docker > /dev/null; then
  curl -fsSL https://get.docker.com | sh
else
  echo "Docker ya está instalado."
fi
systemctl enable --now docker > /dev/null 2>&1 || true
systemctl enable --now cron > /dev/null 2>&1 || true

paso "3/8 Código de VentSoft"
if [ -d "$DIR_BACK/.git" ]; then git -C "$DIR_BACK" pull --ff-only; else git clone https://github.com/fredclrs/VentSoft-Backend.git "$DIR_BACK"; fi
if [ -d "$DIR_FRONT/.git" ]; then git -C "$DIR_FRONT" pull --ff-only; else git clone https://github.com/fredclrs/VentSoft-FrontEnd.git "$DIR_FRONT"; fi

paso "4/8 Archivo .env"
# Solo letras/números + un sufijo fijo con mayúscula, minúscula, número y símbolo: así
# SQL Server siempre la acepta como "fuerte", y no hay caracteres ($ ' " etc.) que
# rompan docker compose o los comandos de sqlcmd.
# (pipefail apagado acá adentro: "head" corta el pipe a propósito y "tr" sale con SIGPIPE.)
clave() { local s; s=$(set +o pipefail; tr -dc 'A-Za-z0-9' < /dev/urandom | head -c "$1"); echo "${s}Aa1_"; }

if [ -f "$DIR_BACK/.env" ]; then
  echo "Ya existe un .env: se conserva tal cual (no se cambian las claves)."
else
  if [ -z "$DOMINIO" ]; then
    IP=$(curl -4 -fsS https://api.ipify.org)
    DOMINIO="${IP//./-}.sslip.io"
  fi
  cat > "$DIR_BACK/.env" <<EOF
MSSQL_SA_PASSWORD=$(clave 24)
APP_DB_USER=ventsoft_app
APP_DB_PASSWORD=$(clave 24)
JWT_KEY=$(clave 60)
ENCRYPTION_KEY=$(clave 60)
SITE_DOMAIN=$DOMINIO
EOF
  chmod 600 "$DIR_BACK/.env"
  echo ".env creado con claves nuevas."
fi
DOMINIO=$(grep '^SITE_DOMAIN=' "$DIR_BACK/.env" | cut -d= -f2-)

paso "5/8 Firewall"
ufw allow OpenSSH > /dev/null
ufw allow 80/tcp > /dev/null
ufw allow 443/tcp > /dev/null
ufw --force enable > /dev/null
echo "Abiertos: 22 (SSH), 80 y 443 (web). Todo lo demás cerrado."

paso "6/8 Levantando VentSoft (la primera vez tarda varios minutos)"
cd "$DIR_BACK"
docker compose up -d --build

paso "7/8 Copia nocturna de la base"
mkdir -p "$DIR_COPIAS"
cat > /usr/local/bin/ventsoft-backup <<'EOF'
#!/bin/bash
# Copia completa de la base VentSoft a /root/ventsoft-backups (guarda las últimas 7).
set -euo pipefail
cd /root/VentSoft-Backend
SAPW=$(grep '^MSSQL_SA_PASSWORD=' .env | cut -d= -f2-)
ARCHIVO="VentSoft_$(date +%Y-%m-%d_%H%M).bak"
docker exec ventsoft-db mkdir -p /var/opt/mssql/backup
docker exec ventsoft-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SAPW" -C -b \
  -Q "BACKUP DATABASE VentSoft TO DISK = N'/var/opt/mssql/backup/ultima.bak' WITH INIT, FORMAT" > /dev/null
docker cp ventsoft-db:/var/opt/mssql/backup/ultima.bak "/root/ventsoft-backups/$ARCHIVO"
docker exec ventsoft-db rm -f /var/opt/mssql/backup/ultima.bak
ls -1t /root/ventsoft-backups/VentSoft_*.bak | tail -n +8 | xargs -r rm -f
echo "$(date '+%F %T') Copia OK: /root/ventsoft-backups/$ARCHIVO"
EOF
chmod +x /usr/local/bin/ventsoft-backup
# 07:00 UTC = 03:00 en Latinoamérica (UTC-4): con el negocio cerrado.
echo '0 7 * * * root /usr/local/bin/ventsoft-backup >> /var/log/ventsoft-backup.log 2>&1' > /etc/cron.d/ventsoft-backup

paso "8/8 Comandos de administración"
cat > /usr/local/bin/ventsoft-estado <<'EOF'
#!/bin/bash
cd /root/VentSoft-Backend && docker compose ps
echo; free -h; echo; df -h /
echo; echo "Últimas copias:"; ls -1t /root/ventsoft-backups 2>/dev/null | head -3
EOF
cat > /usr/local/bin/ventsoft-reiniciar <<'EOF'
#!/bin/bash
cd /root/VentSoft-Backend && docker compose restart && docker compose ps
EOF
cat > /usr/local/bin/ventsoft-actualizar <<'EOF'
#!/bin/bash
# Baja la última versión de los 2 repos y reconstruye. Los datos no se tocan.
set -e
git -C /root/VentSoft-FrontEnd pull --ff-only
git -C /root/VentSoft-Backend pull --ff-only
cd /root/VentSoft-Backend && docker compose up -d --build && docker compose ps
EOF
chmod +x /usr/local/bin/ventsoft-estado /usr/local/bin/ventsoft-reiniciar /usr/local/bin/ventsoft-actualizar

echo
echo "=============================================================="
echo " VentSoft instalado."
echo " Entrá en:   https://$DOMINIO"
echo "             (el certificado HTTPS puede tardar 1-2 minutos)"
echo " Usuario:    admin    Contraseña: admin123  (cambiarla al entrar)"
echo
echo " Comandos:   ventsoft-estado | ventsoft-reiniciar"
echo "             ventsoft-actualizar | ventsoft-backup"
echo " Claves:     guardadas en $DIR_BACK/.env (copialas a un lugar seguro)"
echo "=============================================================="
