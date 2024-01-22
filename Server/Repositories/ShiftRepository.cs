using ShiftTool.Server.Data;
using ShiftTool.Shared.Interfaces;
using ShiftTool.Shared.Models;
using Dapper;

namespace ShiftTool.Server.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;

        public ShiftRepository(INpgsqlConnectionFactory npgsqlConnectionFactory)
        {
            _npgsqlConnectionFactory = npgsqlConnectionFactory ?? throw new ArgumentNullException(nameof(npgsqlConnectionFactory));
        }

        // Hent en vagt med en given ID fra databasen
        public async Task<Shift> GetShiftByIdAsync(int shiftId)
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            return await connection.QueryFirstOrDefaultAsync<Shift>("SELECT * FROM Shifts WHERE ShiftId = @ShiftId", new { ShiftId = shiftId });
        }

        // Hent tilgængelige vagter fra databasen
        public async Task<IEnumerable<Shift>> GetAvailableShiftsAsync()
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            return await connection.QueryAsync<Shift>("SELECT * FROM Shifts WHERE IsBooked = false");
        }

        // Hent vagter sorteret efter prioritet fra databasen
        public async Task<IEnumerable<Shift>> GetShiftsSortedByPriorityAsync()
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            return await connection.QueryAsync<Shift>("SELECT * FROM Shifts ORDER BY Priority ASC");
        }

        // Hent vagter sorteret efter ShiftId i stigende rækkefølge
        public async Task<IEnumerable<Shift>> GetAllShiftsAsync()
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            var query = @" SELECT s.*, b.* FROM Shifts s LEFT JOIN Bookings b ON s.ShiftId = b.ShiftId ORDER BY s.ShiftId ASC";
            var shifts = await connection.QueryAsync<Shift, Booking, Shift>(
                query,
                (shift, booking) =>
                {
                    shift.Booking = booking; // Antager at Shift klassen har en Booking property
                    return shift;
                },
                splitOn: "BookingId" // Eller det korrekte kolonnenavn for Booking ID i din database
            );
            return shifts;
        }


        // Opret en ny vagt i databasen
        public async Task CreateShiftAsync(Shift shift)
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            await connection.ExecuteAsync(@"
                INSERT INTO Shifts (Title, Description, StartDateTime, EndDateTime, Priority, IsBooked)
                VALUES (@Title, @Description, @StartDateTime, @EndDateTime, @Priority, @IsBooked)", shift);
        }

        // Opdater en eksisterende vagt i databasen
        public async Task UpdateShiftAsync(Shift shift)
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            await connection.ExecuteAsync(@"
                UPDATE Shifts
                SET Title = @Title, Description = @Description, StartDateTime = @StartDateTime, EndDateTime = @EndDateTime, Priority = @Priority, IsBooked = @IsBooked
                WHERE ShiftId = @ShiftId", shift);
        }

        // Slet en vagt med en given ID fra databasen
        public async Task DeleteShiftAsync(int shiftId)
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();

            // Slet først alle bookinger relateret til denne vagt
            await connection.ExecuteAsync("DELETE FROM bookings WHERE ShiftId = @ShiftId", new { ShiftId = shiftId });

            // Slet derefter vagten
            await connection.ExecuteAsync("DELETE FROM shifts WHERE ShiftId = @ShiftId", new { ShiftId = shiftId });
        }

    }
}
