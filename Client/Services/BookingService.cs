using ShiftTool.Shared.DTOs;
using ShiftTool.Shared.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShiftTool.Client.Services
{
    public class BookingService : IBookingService
    {
        private readonly HttpClient _httpClient;

        public BookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Hent alle bookinger
        public async Task<IEnumerable<BookingDTO>> GetAllBookingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<BookingDTO>>("api/booking/all");
        }

        // Hent en specifik booking baseret på dens ID
        public async Task<BookingDTO> GetBookingByIdAsync(int bookingId)
        {
            return await _httpClient.GetFromJsonAsync<BookingDTO>($"api/booking/{bookingId}");
        }

        public async Task<IEnumerable<BookingDTO>> GetUserBookingsAsync(string email)
        {
            // Hent alle bookinger for en bestemt bruger
            return await _httpClient.GetFromJsonAsync<IEnumerable<BookingDTO>>($"api/booking/user/{email}");
        }

        // Opret en ny booking
        public async Task CreateBookingAsync(BookingDTO bookingDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/booking/create-booking", bookingDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
              
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new InvalidOperationException($"Fejl ved booking: {errorContent}");
                }
                else
                {
                    throw new InvalidOperationException($"API request failed: {response.ReasonPhrase}. Response content: {errorContent}");
                }
            }
        }

        // Opdater en eksisterende booking
        public async Task UpdateBookingAsync(int bookingId, BookingDTO bookingDto)
        {
            await _httpClient.PutAsJsonAsync($"api/booking/update/{bookingId}", bookingDto);
        }

        // Slet en booking
        public async Task DeleteBookingAsync(int bookingId)
        {
            await _httpClient.DeleteAsync($"api/booking/delete/{bookingId}");
        }
    }
}
