using Microsoft.Data.Sqlite;
using Restaurant.RestApi.Persistence;
using Restaurant.RestApi.Persistence.Repositories;
using SQLitePCL;

namespace Restaurant.RestApi;

internal static class MigrationTool
{
    public static void Migrate(string connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        Batteries.Init();
        var schemaSql = SqlReservationRepository.DbSchema;
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        ArgumentException.ThrowIfNullOrEmpty(schemaSql);
        using var command = new SqliteCommand(schemaSql, connection);
        command.ExecuteNonQuery();
    }
}