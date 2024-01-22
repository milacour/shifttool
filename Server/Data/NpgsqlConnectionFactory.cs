using Npgsql;
using System;

namespace ShiftTool.Server.Data
{
    // Dette interface definerer en metode til at oprette en Npgsql forbindelse.
    public interface INpgsqlConnectionFactory
    {
        NpgsqlConnection CreateNpgsqlConnection();
    }

    // Dette er implementeringen af INpgsqlConnectionFactory.
    public class NpgsqlConnectionFactory : INpgsqlConnectionFactory
    {
        private readonly string _connectionString;

        // Konstruktøren modtager en forbindelsesstreng som parameter.
        public NpgsqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Denne metode opretter og returnerer en ny Npgsql forbindelse baseret på den gemte forbindelsesstreng.
        public NpgsqlConnection CreateNpgsqlConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
