using ShiftTool.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShiftTool.Shared.Interfaces
{
    public interface IShiftService
    {
        Task<ShiftDTO> GetShiftByIdAsync(int shiftId);
        Task<IEnumerable<ShiftDTO>> GetAvailableShiftsAsync();
        Task<IEnumerable<ShiftDTO>> GetShiftsSortedByPriorityAsync();
        Task<IEnumerable<ShiftDTO>> GetAllShiftsAsync();
        Task CreateShiftAsync(ShiftDTO shiftDto);
        Task UpdateShiftAsync(int shiftId, ShiftDTO shiftDto);
        Task DeleteShiftAsync(int shiftId);
    }
}
