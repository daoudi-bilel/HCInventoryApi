using ITInventoryManagementAPI.Models;
using ITInventoryManagementAPI.Models.Responses;
using ITInventoryManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ITInventoryManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<Device>>> GetDevices(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var pagedResponse = await _deviceService.GetDevicesAsync(pageNumber, pageSize);
            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return device;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevice(int id, Device device)
        {
            var updatedDevice = await _deviceService.UpdateDeviceAsync(id, device);
            if (updatedDevice == null)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            var createdDevice = await _deviceService.CreateDeviceAsync(device);
            return CreatedAtAction("GetDevice", new { id = createdDevice.Id }, createdDevice);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var result = await _deviceService.DeleteDeviceAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResponse<Device>>> SearchDevices(string searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var pagedResponse = await _deviceService.SearchDevicesByDescriptionOrTypeAsync(searchTerm, pageNumber, pageSize);
            return Ok(pagedResponse);
        }

        [HttpPut("{deviceId}/link-employee/{employeeId}")]
        public async Task<IActionResult> LinkDeviceToEmployee(int deviceId, int employeeId)
        {
            var result = await _deviceService.LinkDeviceToEmployeeAsync(deviceId, employeeId);
            if (!result)
            {
                return NotFound("Device or Employee not found.");
            }

            return NoContent();
        }
    }
}
