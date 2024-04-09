using ITInventoryManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITInventoryManagementAPI.Models.Responses;


namespace ITInventoryManagementAPI.Services
{
    public interface IEmployeeService
    {
       Task<PagedResponse<Employee>> GetEmployeesAsync(int pageNumber = 1, int pageSize = 10);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<IEnumerable<Employee>> SearchEmployeesByNameOrEmailAsync(string searchTerm);
    }
}
