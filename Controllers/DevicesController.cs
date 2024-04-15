using ITInventoryManagementAPI.Models;
using ITInventoryManagementAPI.Models.Responses;
using ITInventoryManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations; // Add this line for Swagger annotations
using Microsoft.AspNetCore.Http;

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
        [SwaggerOperation(Summary = "Get all devices with pagination and optional keyword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<Device>>> GetDevices(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
           [FromQuery] string? keyword = null,
            [FromQuery] string? sortOrder = null)
        {
            keyword ??= "";
            sortOrder ??= "ASC";
            var pagedResponse = await _deviceService.GetDevicesAsync(page, size, sortOrder, keyword);
            return Ok(pagedResponse);
        }
        
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a device by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Device>> GetDevice(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null)
            {
                return NotFound();
            }
            return Ok(device);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing device")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [SwaggerOperation(Summary = "Create a new device")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Device>> PostDevice(Device device)
        {
            var createdDevice = await _deviceService.CreateDeviceAsync(device);
            return CreatedAtAction(nameof(GetDevice), new { id = createdDevice.Id }, createdDevice);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a device by ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var result = await _deviceService.DeleteDeviceAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
