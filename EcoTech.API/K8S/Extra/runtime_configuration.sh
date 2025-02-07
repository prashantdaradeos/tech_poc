#!/bin/bash

# Ensure required environment variables are set
if [ -z "$ACCEPT_EULA" ] || [ -z "$MSSQL_PID" ] || [ -z "$SA_PASSWORD" ]; then
    echo "Required environment variables (ACCEPT_EULA, MSSQL_PID, SA_PASSWORD) are not set."
    exit 1
fi

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start
sleep 30

# Install sqlcmd tools and dependencies at runtime
apt-get update && apt-get install -y mssql-tools unixodbc-dev curl

# Run the SQL script to create databases or perform other tasks
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "$SA_PASSWORD" -i /scripts/init.sql

# Clean up installed dependencies to reduce the container size
apt-get remove --purge -y mssql-tools unixodbc-dev curl && \
apt-get autoremove -y && \
rm -rf /var/lib/apt/lists/* /scripts/*

wait
