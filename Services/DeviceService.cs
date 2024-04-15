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

       public async Task<PagedResponse<Device>> GetDevicesAsync(int page = 1, int size = 10, string sortOrder = "ASC", string keyword = "")
        {
            var skip = (page - 1) * size;
            IQueryable<Device> query = _context.Devices;

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(d => d.Description.Contains(keyword) || d.Type.Contains(keyword));
            }

            if (sortOrder.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(d => d.Description); 
            }
            else
            {
                query = query.OrderBy(d => d.Description); 
            }

            var devices = await query.Skip(skip).Take(size).ToListAsync();
            var totalItems = await _context.Devices.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / size);

            return new PagedResponse<Device>
            {
                Content = devices,
                TotalItems = totalItems,
                TotalPages = totalPages,
                page = page,
                size = size
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

            if (device.EmployeeId != null && device.EmployeeId != employeeId)
            {
                return false;
            }

            device.EmployeeId = employeeId;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
