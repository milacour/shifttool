using ShiftTool.Server.Data;
using ShiftTool.Shared.Interfaces;
using ShiftTool.Shared.Models;
using Dapper;

namespace ShiftTool.Server.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly INpgsqlConnectionFactory _npgsqlConnectionFactory;

        public BookingRepository(INpgsqlConnectionFactory npgsqlConnectionFactory)
        {
            _npgsqlConnectionFactory = npgsqlConnectionFactory ?? throw new ArgumentNullException(nameof(npgsqlConnectionFactory));
        }

        // Hent alle reservationer fra databasen
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            return await connection.QueryAsync<Booking>("SELECT * FROM Bookings");
        }

        // Hent en reservation baseret på bookingens ID
        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            return await connection.QueryFirstOrDefaultAsync<Booking>("SELECT * FROM Bookings WHERE BookingId = @BookingId", new { BookingId = bookingId });
        }

        // Hent en booket vagt
        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string email)
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            return await connection.QueryAsync<Booking>("SELECT * FROM Bookings WHERE Email = @Email", new { Email = email });
        }

        public async Task CreateBookingAsync(Booking booking)
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Kontroller om ShiftId allerede er booket
                var isBooked = await connection.QueryFirstOrDefaultAsync<bool>(
                    "SELECT IsBooked FROM Shifts WHERE ShiftId = @ShiftId",
                    new { ShiftId = booking.ShiftId });

                if (isBooked)
                {
                    throw new InvalidOperationException("Denne vagt er allerede booket.");
                }

                await connection.ExecuteAsync(@"INSERT INTO Bookings (Email, ShiftId, BookedAt)
                     VALUES (@Email, @ShiftId, @BookedAt)", booking, transaction: transaction);

                await connection.ExecuteAsync("UPDATE Shifts SET IsBooked = true WHERE ShiftId = @ShiftId",
                    new { ShiftId = booking.ShiftId }, transaction: transaction);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Fejl ved oprettelse af booking", ex);
            }
        }


        // Opdater en eksisterende reservation i databasen
        public async Task UpdateBookingAsync(Booking booking)
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            await connection.ExecuteAsync(@"
                UPDATE Bookings
                SET Email = @Email, ShiftId = @ShiftId, BookedAt = @BookedAt
                WHERE BookingId = @BookingId", booking);
        }

        // Slet en reservation fra databasen baseret på bookingens ID
        public async Task DeleteBookingAsync(int bookingId)
        {
            using var connection = _npgsqlConnectionFactory.CreateNpgsqlConnection();
            await connection.ExecuteAsync("DELETE FROM Bookings WHERE BookingId = @BookingId", new { BookingId = bookingId });
        }
    }
}
