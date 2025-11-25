# Points API Sample

Dotnet application to use as a starter for a web API using an MSSQL db. 

MSSQL db startup instructions: https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver17&tabs=cli&pivots=cs1-bash
DBeaver CE db client: https://dbeaver.io/about/

Once dotnet installed, execute '''dotnet run''' in the terminal in the PointsAPI directory. localhost:5025 should retrieve any points uploaded to the db (you'd need to populate the db yourself). Try adding a new data type via the SQL client and hooking up CRUD fucntionality in Models.cs and Program.cs.
