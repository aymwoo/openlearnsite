#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
CONTAINER_NAME="learnsite-mssql"
SA_PASSWORD="YourStrong!Passw0rd"
DB_NAME="learnsite"
MSSQL_IMAGE="mcr.azure.cn/mssql/server:2022-latest"
SQL_INIT_FILE="$SCRIPT_DIR/sql/learnsite.sql"
BACKUP_DIR="$SCRIPT_DIR/backupdb"
BACKUP_MOUNT_DIR="/app/backupdb"

echo "Starting development environment setup for Arch Linux..."

# 1. Install Docker if not installed
if ! command -v docker &> /dev/null; then
    echo "Docker is not installed. Installing Docker..."
    sudo pacman -Sy --noconfirm docker docker-compose
else
    echo "Docker is already installed."
fi

# 2. Ensure Docker service is enabled and started
echo "Starting Docker service..."
sudo systemctl enable --now docker

# 3. Configure Chinese mirror for Docker (speeds up image pulls in China)
DOCKER_DAEMON_JSON="/etc/docker/daemon.json"
NEED_RESTART=false

if [ ! -f "$DOCKER_DAEMON_JSON" ] || ! grep -q "registry-mirrors" "$DOCKER_DAEMON_JSON" 2>/dev/null; then
    echo "Configuring Docker to use Chinese registry mirrors..."
    sudo mkdir -p /etc/docker
    sudo tee $DOCKER_DAEMON_JSON > /dev/null <<EOF
{
  "registry-mirrors": [
    "https://docker.m.daocloud.io",
    "https://dockerproxy.com",
    "https://docker.nju.edu.cn",
    "https://mirror.baidubce.com"
  ]
}
EOF
    NEED_RESTART=true
fi

if [ "$NEED_RESTART" = true ]; then
    echo "Restarting Docker service to apply configuration..."
    sudo systemctl restart docker
fi

# 4. Check if container already exists
if sudo docker ps -a --format '{{.Names}}' | grep -q "^${CONTAINER_NAME}$"; then
    if sudo docker ps --format '{{.Names}}' | grep -q "^${CONTAINER_NAME}$"; then
        echo "Container '$CONTAINER_NAME' is already running."
    else
        echo "Container '$CONTAINER_NAME' exists but is stopped. Starting it..."
        sudo docker start "$CONTAINER_NAME"
    fi
    echo "=================================================="
    echo "MSSQL Server is running on localhost:1433."
    echo "Database: $DB_NAME"
    echo "Username: sa"
    echo "Password: $SA_PASSWORD"
    echo "=================================================="
    exit 0
fi

# 5. Pull official MSSQL Server image
echo "Pulling MSSQL image ($MSSQL_IMAGE)..."
sudo docker pull $MSSQL_IMAGE

# 5.1 Ensure backup directory exists on host
mkdir -p "$BACKUP_DIR"

# 6. Start MSSQL container
echo "Starting MSSQL container..."
sudo docker run -d \
  --name "$CONTAINER_NAME" \
  -e 'ACCEPT_EULA=Y' \
  -e "MSSQL_SA_PASSWORD=$SA_PASSWORD" \
  -p 1433:1433 \
  --restart unless-stopped \
  -v "$BACKUP_DIR:$BACKUP_MOUNT_DIR" \
  $MSSQL_IMAGE

# 7. Wait for SQL Server to be ready
echo "Waiting for SQL Server to start (this may take 30-60 seconds)..."
MAX_RETRIES=30
RETRY_COUNT=0
until sudo docker exec "$CONTAINER_NAME" /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" &> /dev/null; do
    RETRY_COUNT=$((RETRY_COUNT + 1))
    if [ $RETRY_COUNT -ge $MAX_RETRIES ]; then
        echo "ERROR: SQL Server did not start within expected time."
        echo "Check container logs: sudo docker logs $CONTAINER_NAME"
        exit 1
    fi
    echo "  SQL Server not ready yet, retrying in 3 seconds... ($RETRY_COUNT/$MAX_RETRIES)"
    sleep 3
done
echo "SQL Server is ready."

# 8. Create database and initialize schema
echo "Creating database '$DB_NAME'..."
sudo docker exec "$CONTAINER_NAME" /opt/mssql-tools18/bin/sqlcmd \
    -S localhost -U sa -P "$SA_PASSWORD" -C \
    -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'$DB_NAME') CREATE DATABASE [$DB_NAME];"

if [ -f "$SQL_INIT_FILE" ]; then
    echo "Initializing database schema from sql/learnsite.sql..."
    sudo docker cp "$SQL_INIT_FILE" "$CONTAINER_NAME":/tmp/learnsite.sql
    sudo docker exec "$CONTAINER_NAME" /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "$SA_PASSWORD" -C \
        -d "$DB_NAME" -i /tmp/learnsite.sql
    echo "Database schema initialized successfully."
else
    echo "WARNING: SQL init file not found at $SQL_INIT_FILE"
    echo "You can initialize the database later via the /upgrade.aspx page."
fi

echo "=================================================="
echo "Development environment setup complete!"
echo "MSSQL Server is running on localhost:1433."
echo "Database: $DB_NAME"
echo "Username: sa"
echo "Password: $SA_PASSWORD"
echo "Default admin login: admin / 12345"
echo "=================================================="
echo ""
echo "启动 Web 应用:  bash start_web.sh"
echo ""
echo "To stop the container:  sudo docker stop $CONTAINER_NAME"
echo "To start it again:      sudo docker start $CONTAINER_NAME"
echo "To remove it completely: sudo docker rm -f $CONTAINER_NAME"
echo "To view logs:            sudo docker logs $CONTAINER_NAME"
