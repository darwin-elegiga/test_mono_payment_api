#!/bin/bash

db2 connect to testdb

echo "DB2 Checking Schema"

if db2 "SELECT schemaname FROM syscat.schemata WHERE schemaname = 'VPAYBRDDTA'" | grep -q VPAYBRDDTA; then
    echo "$0: Schema Exists Done"
else
    echo "Schema Does Not Exist so Initialize Database"
    db2 "create schema VPAYBRDDTA"
    db2 -tvf /InitializeDatabase.sql
    
fi

echo "$0: DB2 Server Database ready"
touch /tmp/healthy
