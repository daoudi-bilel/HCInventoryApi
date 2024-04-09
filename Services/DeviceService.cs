using ITInventoryManagementAPI.Models;
using ITInventoryManagementAPI.Models.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITInventoryManagementAPI.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly ITInventoryContext _context;

        public DeviceService(ITInventoryContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<Device>> GetDevicesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var skip = (pageNumber - 1) * pageSize;
            var devices = await _context.Devices
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var totalItems = await _context.Devices.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new PagedResponse<Device>
            {
                Content = devices,
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Device> GetDeviceByIdAsync(int id)
        {
            return await _context.Devices.FindAsync(id);
        }

        public async Task<Device> CreateDeviceAsync(Device device)
        {
            _context.Devices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<Device> UpdateDeviceAsync(int id, Device device)
        {
            if (id != device.Id)
            {
                return null;
            }

            _context.Entry(device).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return device;
        }

        public async Task<bool> DeleteDeviceAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResponse<Device>> SearchDevicesByDescriptionOrTypeAsync(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var skip = (pageNumber - 1) * pageSize;
            var devices = await _context.Devices
                .Where(d => d.Description.Contains(searchTerm) || d.Type.Contains(searchTerm))
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var totalItems = await _context.Devices
                .Where(d => d.Description.Contains(searchTerm) || d.Type.Contains(searchTerm))
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new PagedResponse<Device>
            {
                Content = devices,
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

         public async Task<bool> LinkDeviceToEmployeeAsync(int deviceId, int employeeId)
        {
            var device = await _context.Devices.FindAsync(deviceId);
            if (device == null)
            {
                return false;
            }

            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                return false;
            }

            device.EmployeeId = employeeId;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
