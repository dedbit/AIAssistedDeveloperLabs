# DevContainer

SQL container for testing sql connection.

Also works with ssms, when enabling trust server certificate under options


# docker compose 

```powershell
docker-compose up --build -d
docker-compose.exe up -d
docker-compose.exe down

```

# Connect

Connect to database container (Connect from host): 

```bash
docker exec -it dbmigration_devcontainer-database-1 /bin/bash

docker ps
docker exec -it 06bde6963c9e /bin/bash
ls /usr/devcontainer/scripts

/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/01_sqlserver_create.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/02_sqlserver_populate_author.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/03_sqlserver_populate_publisher.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/04_sqlserver_populate_lookups.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/05_sqlserver_populate_book.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/06_sqlserver_populate_bookauthor.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/07_sqlserver_populate_country.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/08_sqlserver_populate_address.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/09_sqlserver_populate_customer.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/10_sqlserver_populate_others.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/11_sqlserver_populate_order.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/12_sqlserver_populate_orderline.sql
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/13_sqlserver_populate_orderhistory.sql

```

# Load Adventureworks database

## From container

Load Adventureworks database from container and drop database. 

```bash
cd /var/opt/mssql/data
/opt/mssql-tools/bin/sqlcmd -U sa -P 'quoosugTh5' -S localhost,1433 -Q "USE master;SELECT name from sys.databases"
```


See StartSqlServer.md for manual startup and load of sql container.

[StartSqlServer.md](../Integrations/sql/StartSqlServer.md)

# Debugging dockerfile changes

Build container and login interactively. Container will be removed when exiting. This way changes can be tested in container quickly and trying again in a short cycle. 

```bash
docker build -t dbmigratedb . -f .\DockerfileSqlServer ; docker run -it -p 6000:6000 --rm dbmigratedb /bin/bash
```

When signed in to the container, Dockerfile RUN commands can be tested. 

Observation: When running "sqlservr &" in Dockerfile and building container using 'docker build' the sqlservr is not running on startup. But with the same change to Dockerfile and using docker-compose.yml the sqlservr is running on startup.


# Setup SSH between containers

```bash
passwd
apt-get update
apt-get install vim  -y
apt-get install openssh-client openssh-server -y
vim /etc/ssh/sshd_config 

# Change or add the line "PermitRootLogin yes"
:q #To exit vim

service ssh restart

ssh root@database
```

