/* using Microsoft.Data.SqlClient;
using PointsApi.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/points", async () =>
{
    var points = new List<Point>();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();
    string c = Utils.TestSqlCommand();
    var command = new SqlCommand(c, connection);
    using var reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        points.Add(new Point
        {
            Id = reader.GetString(0),
            X = reader.GetInt32(1),
            Y = reader.GetInt32(2),
            Z = reader.GetInt32(3)
        });
    }

    return Results.Ok(points);
});

app.Run();

class Utils {
	public static string TestSqlCommand() {
		// TODO: clean and verify sql command
		return "SELECT id, x, y, z FROM points";
	}
}

*/

using Microsoft.Data.SqlClient;
using PointsApi.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string GetConnString() =>
    builder.Configuration.GetConnectionString("DefaultConnection");

// -------------------------------------------------------------------
// GET /points  → return all points
// -------------------------------------------------------------------
app.MapGet("/points", async () =>
{
    var points = new List<Point>();

    using var connection = new SqlConnection(GetConnString());
    await connection.OpenAsync();

    string sql = "SELECT Id, X, Y, Z FROM Points";
    using var command = new SqlCommand(sql, connection);
    using var reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        points.Add(new Point
        {
            Id = reader.GetString(0),
            X = reader.GetInt32(1),
            Y = reader.GetInt32(2),
            Z = reader.GetInt32(3)
        });
    }

    return Results.Ok(points);
});

// -------------------------------------------------------------------
// GET /points/{id}  → return a single point by id
// -------------------------------------------------------------------
app.MapGet("/points/{id}", async (string id) =>
{
    using var connection = new SqlConnection(GetConnString());
    await connection.OpenAsync();

    string sql = "SELECT Id, X, Y, Z FROM Points WHERE Id = @id";
    using var command = new SqlCommand(sql, connection);

    command.Parameters.AddWithValue("@id", id);

    using var reader = await command.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
        return Results.NotFound();

    var point = new Point
    {
        Id = reader.GetString(0),
        X = reader.GetInt32(1),
        Y = reader.GetInt32(2),
        Z = reader.GetInt32(3)
    };

    return Results.Ok(point);
});

// -------------------------------------------------------------------
// POST /points  → create a new point
// -------------------------------------------------------------------
app.MapPost("/points", async (Point point) =>
{
    using var connection = new SqlConnection(GetConnString());
    await connection.OpenAsync();

    string sql = @"INSERT INTO Points (Id, X, Y, Z) 
                   VALUES (@id, @x, @y, @z)";

    using var command = new SqlCommand(sql, connection);

    command.Parameters.AddWithValue("@id", point.Id);
    command.Parameters.AddWithValue("@x", point.X);
    command.Parameters.AddWithValue("@y", point.Y);
    command.Parameters.AddWithValue("@z", point.Z);

    await command.ExecuteNonQueryAsync();

    return Results.Created($"/points/{point.Id}", point);
});

// -------------------------------------------------------------------
// PUT /points/{id}  → update a point
// -------------------------------------------------------------------
app.MapPut("/points/{id}", async (string id, Point updated) =>
{
    using var connection = new SqlConnection(GetConnString());
    await connection.OpenAsync();

    string sql = @"UPDATE Points 
                   SET X = @x, Y = @y, Z = @z 
                   WHERE Id = @id";

    using var command = new SqlCommand(sql, connection);

    command.Parameters.AddWithValue("@id", id);
    command.Parameters.AddWithValue("@x", updated.X);
    command.Parameters.AddWithValue("@y", updated.Y);
    command.Parameters.AddWithValue("@z", updated.Z);

    int rows = await command.ExecuteNonQueryAsync();

    return rows == 0 ? Results.NotFound() : Results.NoContent();
});

// -------------------------------------------------------------------
// DELETE /points/{id}  → delete a point
// -------------------------------------------------------------------
app.MapDelete("/points/{id}", async (string id) =>
{
    using var connection = new SqlConnection(GetConnString());
    await connection.OpenAsync();

    string sql = "DELETE FROM Points WHERE Id = @id";
    using var command = new SqlCommand(sql, connection);

    command.Parameters.AddWithValue("@id", id);

    int rows = await command.ExecuteNonQueryAsync();

    return rows == 0 ? Results.NotFound() : Results.NoContent();
});

app.Run();


// -------------------------------------------------------------------
// UTILS — you can keep these here or move to a separate file later
// -------------------------------------------------------------------
class Utils 
{
    public static string TestSqlCommand() 
    {
        // TODO: clean and verify sql command
        return "SELECT Id, X, Y, Z FROM Points";
    }
}


