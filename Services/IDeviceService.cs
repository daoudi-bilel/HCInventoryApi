using ITInventoryManagementAPI.Models;
using ITInventoryManagementAPI.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITInventoryManagementAPI.Services
{
    public interface IDeviceService
    {
        Task<PagedResponse<Device>> GetDevicesAsync(int page = 1, int size = 10, string sortOrder = "ASC", string keyword = "");
        Task<Device> GetDeviceByIdAsync(int id);
        Task<Device> CreateDeviceAsync(Device device);
        Task<Device> UpdateDeviceAsync(int id, Device device);
        Task<bool> DeleteDeviceAsync(int id);
        Task<bool> LinkDeviceToEmployeeAsync(int deviceId, int employeeId);
    }
}
