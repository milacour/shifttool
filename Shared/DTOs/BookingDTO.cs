using ShiftTool.Shared.Models;

namespace ShiftTool.Shared.DTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public string Email { get; set; }
        public int ShiftId { get; set; }
        public DateTime BookedAt { get; set; }
    }
}
