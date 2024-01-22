using Blazored.LocalStorage;
using ShiftTool.Shared.Interfaces;
using ShiftTool.Shared.Helpers;
using ShiftTool.Shared.DTOs;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

namespace ShiftTool.Client.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly UserState _userState;
        private readonly NavigationManager _navigationManager;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage, UserState userState, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _userState = userState;
            _navigationManager = navigationManager;
        }

        // Registrerer en ny bruger
        public async Task<HttpResponseMessage> RegisterUser(UserDTO userDto)
        {
            return await _httpClient.PostAsJsonAsync("api/user/register", userDto);
        }

        // Logger en bruger ind
        public async Task<HttpResponseMessage> LoginAsync(UserDTO userDto)
        {
            return await _httpClient.PostAsJsonAsync("api/user/login", userDto);
        }

        // Logger en bruger ud
        public async Task<HttpResponseMessage> LogoutAsync()
        {
            return await _httpClient.PostAsync("api/user/logout", null);
        }

        // Opretter en ny bruger
        public async Task<HttpResponseMessage> CreateUserAsync(UserDTO userDto)
        {
            return await _httpClient.PostAsJsonAsync("api/user/create-user", userDto);
        }

        // Henter alle brugere
        public async Task<List<UserDTO>> GetUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<UserDTO>>("api/user/get-users");
        }

        // Henter den aktuelle bruger baseret på email
        public async Task<UserDTO> GetCurrentUserAsync(string email)
        {
            return await _httpClient.GetFromJsonAsync<UserDTO>($"api/user/get-user/{email}");
        }

        // Opdaterer en bruger
        public async Task<HttpResponseMessage> UpdateUserAsync(string email, UserDTO userDto)
        {
            return await _httpClient.PutAsJsonAsync($"api/user/update-user/{email}", userDto);
        }

        // Sletter en bruger
        public async Task<HttpResponseMessage> DeleteUser(string email)
        {
            return await _httpClient.DeleteAsync($"api/user/delete-user/{email}");
        }

        // Håndterer logik for login efter registrering eller på login-siden
        public async Task<bool> HandleLogin(UserDTO userDto, bool navigateToProfile)
        {
            var httpResponse = await LoginAsync(userDto);

            if (httpResponse.IsSuccessStatusCode)
            {
                var loginResponse = await httpResponse.Content.ReadFromJsonAsync<LoginResponse>();

                if (loginResponse != null && loginResponse.Success)
                {
                    await _localStorage.SetItemAsync("isLoggedIn", true);
                    await _localStorage.SetItemAsync("userEmail", userDto.Email);

                    _userState.Login(loginResponse.UserData);

                    if (navigateToProfile)
                    {
                        _navigationManager.NavigateTo("/profil");
                    }

                    return true;
                }
            }

            return false;
        }

    }
}
