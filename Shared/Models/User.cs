namespace ShiftTool.Shared.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public bool IsCoordinator { get; set; } = false;
        public string Experience { get; set; } = "";
        public string Skills { get; set; } = "";
        public DateTime CreatedAt { get; set; } 
    }
}
