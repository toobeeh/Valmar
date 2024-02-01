#!/bin/bash
set -e
sed -i "s/DB_DOMAIN_NAME_SED_PLACEHOLDER/$DB_DOMAIN_NAME/g" /app/appsettings.json

# Start the main process
exec "$@"