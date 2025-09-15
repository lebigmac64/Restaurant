using Microsoft.Data.Sqlite;

namespace Restaurant.RestApi.Persistence.Repositories;

#pragma warning disable CA1515
public class SqlReservationRepository :
#pragma warning restore CA1515
    IReservationsRepository
{
    public const string DbSchema = @"
    CREATE TABLE IF NOT EXISTS Reservations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    At DATETIME NOT NULL,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL,
    Quantity INTEGER NOT NULL);";
    
    public SqlReservationRepository(string connectionString)
    {
        ConnectionString = connectionString;
    }

    private string ConnectionString { get; }
    
    public async Task Create(Reservation reservation)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        using var conn = new SqliteConnection(ConnectionString);
        using var cmd = new SqliteCommand(CreateReservationSql, conn);
        cmd.Parameters.Add(new SqliteParameter("@At", reservation.At));
        cmd.Parameters.Add(new SqliteParameter("@Name", reservation.Name));
        cmd.Parameters.Add(new SqliteParameter("@Email", reservation.Email));
        cmd.Parameters.Add(new SqliteParameter("@Quantity", reservation.Quantity));
        
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    private const string CreateReservationSql = """
                                                INSERT INTO Reservations (At, Email, Name, Quantity)
                                                VALUES (@At, @Email, @Name, @Quantity)
                                                """;
}