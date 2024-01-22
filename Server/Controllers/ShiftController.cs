using Microsoft.AspNetCore.Mvc;
using ShiftTool.Shared.DTOs;
using ShiftTool.Shared.Interfaces;
using ShiftTool.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShiftTool.Server.Controllers
{
   
    [ApiController]
    [Route("api/shift")]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftRepository _shiftRepository;

        public ShiftController(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        // Henter en vagt baseret på ID
        [HttpGet("{shiftId}")]
        public async Task<ActionResult<ShiftDTO>> GetShiftByIdAsync(int shiftId)
        {
            var shift = await _shiftRepository.GetShiftByIdAsync(shiftId);
            if (shift == null)
            {
                return NotFound("Vagt ikke fundet");
            }

            return Ok(ConvertToDTO(shift));
        }

        // Henter alle tilgængelige vagter
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetAvailableShiftsAsync()
        {
            var shifts = await _shiftRepository.GetAvailableShiftsAsync();
            return Ok(shifts.Select(ConvertToDTO));
        }

        // Henter vagter sorteret efter prioritet
        [HttpGet("priority")]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetShiftsSortedByPriorityAsync()
        {
            var shifts = await _shiftRepository.GetShiftsSortedByPriorityAsync();
            return Ok(shifts.Select(ConvertToDTO));
        }

        // Henter alle vagter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetAllShiftsAsync()
        {
            var shifts = await _shiftRepository.GetAllShiftsAsync();
            return Ok(shifts.Select(ConvertToDTO));
        }

        // Opretter en ny vagt
        [HttpPost("create-shift")]
        public async Task<ActionResult> CreateShiftAsync([FromBody] ShiftDTO shiftDto)
        {
            var shift = new Shift();
            ConvertFromDTO(shiftDto, shift);

            await _shiftRepository.CreateShiftAsync(shift);
            return Ok("Vagt oprettet succesfuldt");
        }

        // Opdaterer en eksisterende vagt
        [HttpPut("update-shift/{shiftId}")]
        public async Task<ActionResult> UpdateShiftAsync(int shiftId, [FromBody] ShiftDTO shiftDto)
        {
            var existingShift = await _shiftRepository.GetShiftByIdAsync(shiftId);
            if (existingShift == null)
            {
                return NotFound("Vagt ikke fundet");
            }

            ConvertFromDTO(shiftDto, existingShift);
            await _shiftRepository.UpdateShiftAsync(existingShift);
            return Ok("Vagt opdateret succesfuldt");
        }

        // Sletter en vagt
        [HttpDelete("delete-shift/{shiftId}")]
        public async Task<ActionResult> DeleteShiftAsync(int shiftId)
        {
            var existingShift = await _shiftRepository.GetShiftByIdAsync(shiftId);
            if (existingShift == null)
            {
                return NotFound("Vagt ikke fundet");
            }

            await _shiftRepository.DeleteShiftAsync(shiftId);
            return Ok("Vagt slettet succesfuldt");
        }

        // Konverterer en Shift-model til en ShiftDTO
        private ShiftDTO ConvertToDTO(Shift shift)
        {
            return new ShiftDTO
            {
                ShiftId = shift.ShiftId,
                Title = shift.Title,
                Description = shift.Description,
                StartDateTime = shift.StartDateTime,
                EndDateTime = shift.EndDateTime,
                Priority = shift.Priority,
                IsBooked = shift.IsBooked
            };
        }

        // Overfører værdier fra en ShiftDTO til en Shift-model
        private void ConvertFromDTO(ShiftDTO shiftDto, Shift shift)
        {
            shift.Title = shiftDto.Title;
            shift.Description = shiftDto.Description;
            shift.StartDateTime = shiftDto.StartDateTime;
            shift.EndDateTime = shiftDto.EndDateTime;
            shift.Priority = shiftDto.Priority;
            shift.IsBooked = shiftDto.IsBooked;
        }
    }
}
