using Microsoft.Data.SqlClient;
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
