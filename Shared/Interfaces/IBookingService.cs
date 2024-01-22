using ShiftTool.Shared.DTOs;

namespace ShiftTool.Shared.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDTO>> GetAllBookingsAsync();
        Task<BookingDTO> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<BookingDTO>> GetUserBookingsAsync(string email);
        Task CreateBookingAsync(BookingDTO bookingDto);
        Task UpdateBookingAsync(int bookingId, BookingDTO bookingDto);
        Task DeleteBookingAsync(int bookingId);
    }
}
