# Points API Sample

Dotnet application to use as a starter for a web API using an MSSQL db. 

MSSQL db startup instructions: https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver17&tabs=cli&pivots=cs1-bash
DBeaver CE db client: https://dbeaver.io/about/

Once dotnet installed, ensure dependencies of PointsApi.csproj are installed, and launch app using:

```bash
dotnet run
```
### Example curl expression for POST and GET requests (confirm localhost port):

1. Create a point:
```bash
curl -X POST http://localhost:9001/points   -H "Content-Type: application/json"   -d '{
    "id": "point1",
    "x": 10,
    "y": 20,
    "z": 30
  }'

```
2. Get all points, get one point:
```bash
curl GET http://localhost:9001/points

```
```bash
curl GET http://localhost:9001/points/point1

```

