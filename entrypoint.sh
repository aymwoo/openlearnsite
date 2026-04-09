#!/bin/bash
set -e

# The default password in web.config is YourStrong!Passw0rd
# We replace it with the DB_PASSWORD environment variable if it's set
if [ -n "$DB_PASSWORD" ]; then
    echo "Updating database password in web.config..."
    sed -i "s/pwd=YourStrong\!Passw0rd;/pwd=${DB_PASSWORD};/g" /app/web.config
fi

echo "Starting XSP4..."
exec xsp4 --port 8080 --root /app --nonstop
