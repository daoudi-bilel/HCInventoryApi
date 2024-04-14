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

       public async Task<PagedResponse<Employee>> GetEmployeesAsync(int page = 1, int size = 10, string sortOrder = "ASC", string keyword = "")
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

            var employees = await query.Skip(skip).Take(size).ToListAsync();
            var totalItems = await _context.Employees.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / size);

            return new PagedResponse<Employee>
            {
                Content = employees,
                TotalItems = totalItems,
                TotalPages = totalPages,
                page = page,
                size = size
            };
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return null;
            }

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return employee;
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
        public async Task<IEnumerable<Employee>> SearchEmployeesByNameOrEmailAsync(string searchTerm)
        {
            return await _context.Employees
                .Where(e => e.Name.Contains(searchTerm) || e.Email.Contains(searchTerm))
                .ToListAsync();
        }
    }

}
