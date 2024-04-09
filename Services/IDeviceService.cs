using ITInventoryManagementAPI.Models;
using ITInventoryManagementAPI.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITInventoryManagementAPI.Services
{
    public interface IDeviceService
    {
        Task<PagedResponse<Device>> GetDevicesAsync(int pageNumber = 1, int pageSize = 10);
        Task<Device> GetDeviceByIdAsync(int id);
        Task<Device> CreateDeviceAsync(Device device);
        Task<Device> UpdateDeviceAsync(int id, Device device);
        Task<bool> DeleteDeviceAsync(int id);
        Task<PagedResponse<Device>> SearchDevicesByDescriptionOrTypeAsync(string searchTerm, int pageNumber = 1, int pageSize = 10);
        Task<bool> LinkDeviceToEmployeeAsync(int deviceId, int employeeId);
    }
}
