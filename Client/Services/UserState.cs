using ShiftTool.Shared.DTOs;
using ShiftTool.Shared.Interfaces;
using Blazored.LocalStorage;

namespace ShiftTool.Client.Services
{
    public class UserState
    {
        private readonly IBookingService bookingService;
        private readonly ILocalStorageService localStorage;
        private readonly IUserService userService;

        public UserDTO CurrentUser { get; private set; }
        public DateTime? SessionExpiry { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;
        public bool IsSessionActive => SessionExpiry.HasValue && DateTime.Now <= SessionExpiry.Value;
        public event Action OnUserStateChanged;

        // Ændr konstruktøren til at tage ILocalStorageService som parameter
        public UserState(IBookingService bookingService, ILocalStorageService localStorage)
        {
            this.bookingService = bookingService;
            this.localStorage = localStorage;
        }

        public async Task Initialize()
        {
            var isLoggedIn = await localStorage.GetItemAsync<bool>("isLoggedIn");
            if (isLoggedIn)
            {
                var sessionExpiryString = await localStorage.GetItemAsync<string>("sessionExpiry");
                var email = await localStorage.GetItemAsync<string>("email");

                if (DateTime.TryParse(sessionExpiryString, out var sessionExpiry) && DateTime.Now <= sessionExpiry && !string.IsNullOrEmpty(email))
                {
                    var userDto = await userService.GetCurrentUserAsync(email); 
                    if (userDto != null)
                    {
                        Login(userDto);
                    }
                }
            }
        }

            public void Login(UserDTO UserData)
        {
            CurrentUser = UserData;
            SessionExpiry = DateTime.Now.AddHours(1);

            localStorage.SetItemAsync("isLoggedIn", true);
            localStorage.SetItemAsync("sessionExpiry", SessionExpiry.ToString());
            localStorage.SetItemAsync("email", UserData.Email);

            OnUserStateChanged?.Invoke();
        }


        public void Logout()
        {
            CurrentUser = null;
            SessionExpiry = null;

            localStorage.RemoveItemAsync("isLoggedIn");
            localStorage.RemoveItemAsync("sessionExpiry");

            OnUserStateChanged?.Invoke();
        }

        public void UpdateUser(UserDTO updatedUser)
        {
            if (CurrentUser != null && CurrentUser.Email == updatedUser.Email)
            {
                CurrentUser = updatedUser;
                OnUserStateChanged?.Invoke();
            }
        }

        public async Task AddBooking(BookingDTO newBooking)
        {
            if (CurrentUser != null)
            {
                var updatedBookings = await bookingService.GetUserBookingsAsync(CurrentUser.Email);
                CurrentUser.Bookings = updatedBookings.ToList();
                OnUserStateChanged?.Invoke();
            }
        }
    }
}