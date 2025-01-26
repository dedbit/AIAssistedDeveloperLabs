# Introduction 

SQL container for testing sql connection.

Also works with ssms, when enabling trust server certificate under options

Using source database: https://www.databasestar.com/sample-bookstore-database/

Github project: https://github.com/cpvariyani/entity-framework-core-codefirst-db-migration.git

# Load Adventureworks database

On host, download the AdventureWorks database to the container_db folder. 


## Start project container using docker (For offline use)

```powershell
cd .\data\
# Start sql docker container with mapping to local folders
docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=quoosugTh5' `
-p 1433:1433 `
-v ${PWD}/data:/var/opt/mssql/data `
-v ${PWD}/log:/var/opt/mssql/log `
-v ${PWD}/secrets:/var/opt/mssql/secrets `
-d mcr.microsoft.com/mssql/server:2022-latest

$port = 6000
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\01_sqlserver_create.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\02_sqlserver_populate_author.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\03_sqlserver_populate_publisher.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\04_sqlserver_populate_lookups.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\05_sqlserver_populate_book.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\06_sqlserver_populate_bookauthor.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\07_sqlserver_populate_country.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\08_sqlserver_populate_address.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\09_sqlserver_populate_customer.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\10_sqlserver_populate_others.sql    
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\11_sqlserver_populate_order.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\12_sqlserver_populate_orderline.sql
sqlcmd -S localhost,$port -U SA -P "quoosugTh5" -i .\scripts\13_sqlserver_populate_orderhistory.sql


# docker build -t dbmigratedb . ; docker run -it -p 6000:6000 --rm dbmigratedb /bin/bash

docker build -t dbmigratedb . -f .\DockerfileSqlServer ; 
docker run -it -p 6000:1433 --rm dbmigratedb /bin/bash
```

In container, startup SQL server
```bash
ps -ef |grep /opt/mssql/bin/sqlservr
/opt/mssql/bin/sqlservr &
```


Connect from host: 
```powershell
sqlcmd -S localhost,6000 -U sa -P 'quoosugTh5' -Q "USE master;SELECT name from sys.databases"
sqlcmd -S localhost,6000 -U SA -P "quoosugTh5" -i LoadAdventureworksDb.sql
```


Import database and check. 

```bash
/opt/mssql-tools/bin/sqlcmd -U sa -P 'quoosugTh5' -S localhost,1433 -Q "USE master;SELECT name from sys.databases"
/opt/mssql-tools/bin/sqlcmd -S localhost,6000 -U SA -P "quoosugTh5" -i LoadAdventureworksDb.sql

/opt/mssql-tools/bin/sqlcmd -S localhost,6000 -U SA -P "quoosugTh5" -i /usr/devcontainer/scripts/01_sqlserver_create.sql
```

Install common tools:

apt-get update && apt-get install -y sudo
apt-get install -y vim

Import bacpac

Worked:
```bash	
cd ~
apt-get update && apt-get install -y unzip
wget https://aka.ms/sqlpackage-linux
mkdir sqlpackage
unzip sqlpackage-linux -d ~/sqlpackage 
echo "export PATH=\"\$PATH:$HOME/sqlpackage\"" >> ~/.bashrc
chmod a+x ~/sqlpackage/sqlpackage
source ~/.bashrc
sqlpackage

cd /app
sqlpackage /Action:Import /SourceFile:container_db/KK_Temp.bacpac  /TargetConnectionString:"Data Source=localhost,6000;User ID=sa; Password=quoosugTh5; Initial Catalog=KK_Temp; Integrated Security=false;TrustServerCertificate=True;"
```

Import bacpac
```
.\sqlpackage\sqlpackage.exe /Action:Import /SourceFile:"c:\Users\newsl\OneDrive\Projekter\KK\opgaver\231124 csvView\Original\KK_Temp.bacpac" /TargetConnectionString:"Data Source=localhost,6000;User ID=sa; Password=quoosugTh5; Initial Catalog=KK_Temp; Integrated Security=false;"
```

Connect from host: 
```powershell
sqlcmd -S localhost,6000 -U sa -P 'quoosugTh5' -Q "USE master;SELECT name from sys.databases"
```

# Entity framework

dotnet tool install --global dotnet-ef


$connectionString = "Server=localhost,1433;Database=gravity_books;User Id=SA;Password=quoosugTh5;"
dotnet ef dbcontext scaffold $connectionString Microsoft.EntityFrameworkCore.SqlServer -o BookModel



# References

[Microsoft SQL Server - Ubuntu based images](https://hub.docker.com/_/microsoft-mssql-server)


