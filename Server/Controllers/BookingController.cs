using Microsoft.AspNetCore.Mvc;
using ShiftTool.Shared.DTOs;
using ShiftTool.Shared.Interfaces;
using ShiftTool.Shared.Models;


namespace ShiftTool.Server.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        // Hent alle bookinger og returner dem som en liste af BookingDTOs
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();
            var bookingDtos = bookings.Select(ConvertToDTO).ToList();
            return Ok(bookingDtos);
        }

        // Hent en specifik booking efter ID og returner den som en BookingDTO
        [HttpGet("{bookingId}")]
        public async Task<ActionResult<BookingDTO>> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            var bookingDto = ConvertToDTO(booking);
            return Ok(bookingDto);

        }

        [HttpGet("user/{email}")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetUserBookingsAsync(string email)
        {
            var bookings = await _bookingRepository.GetUserBookingsAsync(email);
            var bookingDtos = bookings.Select(ConvertToDTO).ToList();
            return Ok(bookingDtos);
        }

        [HttpPost("create-booking")]
        public async Task<ActionResult<string>> CreateBookingAsync([FromBody] BookingDTO bookingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = ConvertFromDTO(bookingDto);
            await _bookingRepository.CreateBookingAsync(booking);
            return Ok("Booking tilføjet med succes");
        }

        // Opdater en eksisterende booking baseret på BookingDTO
        [HttpPut("update/{bookingId}")]
        public async Task<ActionResult<string>> UpdateBookingAsync(int bookingId, [FromBody] BookingDTO bookingDto)
        {
            var existingBooking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (existingBooking == null)
            {
                return NotFound();
            }

            ConvertFromDTO(bookingDto, existingBooking);
            await _bookingRepository.UpdateBookingAsync(existingBooking);
            return Ok("Booking opdateret med succes");
        }

        // Konverter en Booking til en BookingDTO
        private BookingDTO ConvertToDTO(Booking booking)
        {
            return new BookingDTO
            {
                BookingId = booking.BookingId,
                Email = booking.Email,
                ShiftId = booking.ShiftId,
                BookedAt = booking.BookedAt,
                //Overfør Shift-data
                //ShiftTitle = booking.Shift?.Title,
                //ShiftStart = booking.Shift?.StartDateTime,
                //ShiftEnd = booking.Shift?.EndDateTime
            };
        }


        // Konverter en BookingDTO til en Booking
        private Booking ConvertFromDTO(BookingDTO bookingDto)
        {
            return new Booking
            {
                Email = bookingDto.Email,
                ShiftId = bookingDto.ShiftId,
                BookedAt = DateTime.UtcNow
            };
        }

        // Opdater eksisterende Booking med værdier fra BookingDTO
        private void ConvertFromDTO(BookingDTO bookingDto, Booking booking)
        {
            booking.Email = bookingDto.Email;
            booking.ShiftId = bookingDto.ShiftId;
        }
    }
}
