using ShiftTool.Shared.Models;

namespace ShiftTool.Shared.Interfaces
{
    public interface IShiftRepository
    {
        Task<IEnumerable<Shift>> GetAllShiftsAsync();
        Task<Shift> GetShiftByIdAsync(int shiftId);
        Task<IEnumerable<Shift>> GetAvailableShiftsAsync();
        Task<IEnumerable<Shift>> GetShiftsSortedByPriorityAsync();
        Task CreateShiftAsync(Shift shift);
        Task UpdateShiftAsync(Shift shift);
        Task DeleteShiftAsync(int shiftId);
    }
}
