# mssql-cassandra-benchmark

# MSSQL
> docker run --name mssql -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Keyboard_cat1234" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest <br>
> create a database called CodePool <br>
> run msSqlInit function in the project <br>

# Cassandra Setup
> docker run --name cassandra -d -p "9042:9042" -e CASSANDRA_BROADCAST_ADDRESS=192.168.99.100 cassandra:latest <br>
> CREATE KEYSPACE codepool WITH replication = {'class':'SimpleStrategy', 'replication_factor' : 1}; <br>
> Use codepool; <br>
> CREATE TABLE CodePoolCollection(ID bigint PRIMARY KEY, Code text, Status tinyint, CreatedAt date, ModifiedAt date); <br>
