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

        public async Task<PagedResponse<Employee>> GetEmployeesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var skip = (pageNumber - 1) * pageSize;
            var employees = await _context.Employees
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var totalItems = await _context.Employees.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new PagedResponse<Employee>
            {
                Content = employees,
                TotalItems = totalItems,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize
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

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Employee>> SearchEmployeesByNameOrEmailAsync(string searchTerm)
        {
            return await _context.Employees
                .Where(e => e.Name.Contains(searchTerm) || e.Email.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
