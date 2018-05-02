:: Setup database schema and data
:: Setting up useful variables
set server="(LocalDB)\MSSQLLocalDB"
set databaseName="paperrulez"

:: Setting up database
sqlcmd -S %server% -i "./database.sql"

:: Setting up schema
sqlcmd -S %server% -i "./client_table.sql" -d %databaseName%