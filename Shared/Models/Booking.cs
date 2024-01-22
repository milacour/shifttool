namespace ShiftTool.Shared.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string Email { get; set; }
        public int ShiftId { get; set; } 
        public DateTime BookedAt { get; set; }
    }
}
