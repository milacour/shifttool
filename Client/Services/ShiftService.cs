using ShiftTool.Shared.DTOs;
using ShiftTool.Shared.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShiftTool.Client.Services
{
    public class ShiftService : IShiftService
    {
        private readonly HttpClient _httpClient;

        public ShiftService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Hent en specifik vagt baseret på dens ID
        public async Task<ShiftDTO> GetShiftByIdAsync(int shiftId)
        {
            return await _httpClient.GetFromJsonAsync<ShiftDTO>($"api/Shift/{shiftId}");
        }

        // Hent alle tilgængelige vagter
        public async Task<IEnumerable<ShiftDTO>> GetAvailableShiftsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ShiftDTO>>("api/Shift/available");
        }

        // Hent vagter sorteret efter prioritet
        public async Task<IEnumerable<ShiftDTO>> GetShiftsSortedByPriorityAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ShiftDTO>>("api/Shift/priority");
        }

        // Hent alle vagter
        public async Task<IEnumerable<ShiftDTO>> GetAllShiftsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ShiftDTO>>("api/Shift");
        }

        // Opret en ny vagt
        public async Task CreateShiftAsync(ShiftDTO shiftDto)
        {
            await _httpClient.PostAsJsonAsync("api/Shift/create-shift", shiftDto);
        }

        // Opdater en eksisterende vagt
        public async Task UpdateShiftAsync(int shiftId, ShiftDTO shiftDto)
        {
            await _httpClient.PutAsJsonAsync($"api/Shift/update-shift/{shiftId}", shiftDto);
        }

        // Slet en vagt
        public async Task DeleteShiftAsync(int shiftId)
        {
            await _httpClient.DeleteAsync($"api/Shift/delete-shift/{shiftId}");
        }

    }
}
