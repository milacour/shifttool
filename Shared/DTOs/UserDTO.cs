using ShiftTool.Shared.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShiftTool.Shared.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "E-mail er påkrævet")]
        [EmailAddress(ErrorMessage = "Ugyldig e-mail-adresse")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "Kodeord er påkrævet")]
        [MinLength(6, ErrorMessage = "Kodeordet skal være mindst 6 tegn langt")]
        public string Password { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Experience { get; set; } = "";
        public string Skills { get; set; } = "";
        public bool IsCoordinator { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public List<BookingDTO> Bookings { get; set; } = new List<BookingDTO>();
    }
}
