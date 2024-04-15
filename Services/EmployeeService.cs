using ITInventoryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITInventoryManagementAPI.Models.Responses;


namespace ITInventoryManagementAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ITInventoryContext _context;

        public EmployeeService(ITInventoryContext context)
        {
            _context = context;
        }

      public async Task<PagedResponse<EmployeeDto>> GetEmployeesAsync(int page = 1, int size = 10, string sortOrder = "ASC", string keyword = "")
        {
            var skip = (page - 1) * size;
            IQueryable<Employee> query = _context.Employees;

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(e => e.Name.Contains(keyword) || e.Email.Contains(keyword));
            }

            if (sortOrder.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(e => e.Name);
            }
            else
            {
                query = query.OrderBy(e => e.Name);
            }

            var employees = await query
                .Skip(skip)
                .Take(size)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email
                })
                .ToListAsync();

            var totalItems = await _context.Employees.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / size);

            return new PagedResponse<EmployeeDto>
            {
                Content = employees,
                TotalItems = totalItems,
                TotalPages = totalPages,
                page = page,
                size = size
            };
        }


       public async Task<SingleEmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Devices)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                throw new InvalidOperationException($"Employee with ID {id} not found.");
            }

            var employeeDto = new SingleEmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Devices = employee.Devices.Select(d => new DeviceDto
                {
                    Id = d.Id,
                    Type = d.Type,
                    Description = d.Description
                }).ToList()
            };

            return employeeDto;
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, EmployeeDto employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
            {
                throw new InvalidOperationException($"Employee with ID {id} not found.");
            }
            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;

            _context.Entry(existingEmployee).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingEmployee;
        }


      public async Task<DeleteEmployeeResult> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return DeleteEmployeeResult.EmployeeNotFound;
            }
            
            var hasRelatedDevices = await _context.Devices.AnyAsync(d => d.EmployeeId == id);
            if (hasRelatedDevices)
            {
                return DeleteEmployeeResult.HasRelatedDevices;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            
            return DeleteEmployeeResult.Success;
        }

        public async Task UpdateEmployeeDevicesAsync(int employeeId, List<int> deviceIds)
        {
            var existingEmployee = await _context.Employees
                .Include(e => e.Devices)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (existingEmployee == null)
            {
                throw new InvalidOperationException($"Employee with ID {employeeId} not found.");
            }

            // Compare the existing devices with the new device IDs
            var existingDeviceIds = existingEmployee.Devices.Select(d => d.Id).ToList();
            var devicesToAdd = deviceIds.Except(existingDeviceIds).ToList();
            var devicesToRemove = existingDeviceIds.Except(deviceIds).ToList();

            // Add new devices
            foreach (var deviceId in devicesToAdd)
            {
                var device = await _context.Devices.FindAsync(deviceId);
                if (device != null)
                {
                    existingEmployee.Devices.Add(device);
                }
            }

            // Remove devices
            foreach (var deviceId in devicesToRemove)
            {
                var device = existingEmployee.Devices.FirstOrDefault(d => d.Id == deviceId);
                if (device != null)
                {
                    existingEmployee.Devices.Remove(device);
                }
            }

            // Update the employee in the database
            _context.Entry(existingEmployee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


    }

}
