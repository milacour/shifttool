
using ShiftTool.Shared.DTOs;

namespace ShiftTool.Shared.Helpers
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserDTO UserData { get; set; }
    }

}

