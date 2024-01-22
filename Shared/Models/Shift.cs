namespace ShiftTool.Shared.Models
{
    public class Shift
    {
        public int ShiftId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Priority { get; set; }
        public bool IsBooked { get; set; }

        // Relation til Booking (nullable, da en Shift ikke altid er booket)
        public virtual Booking? Booking { get; set; }
    }
}
