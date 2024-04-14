using ITInventoryManagementAPI.Models;
using ITInventoryManagementAPI.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITInventoryManagementAPI.Services
{
    public interface IEmployeeService
    {
        Task<PagedResponse<Employee>> GetEmployeesAsync(int page = 1, int size = 10, string sortOrder = "ASC", string keyword = "");
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);
        Task<DeleteEmployeeResult> DeleteEmployeeAsync(int id);
        Task<IEnumerable<Employee>> SearchEmployeesByNameOrEmailAsync(string searchTerm);
    }
        public enum DeleteEmployeeResult
        {
            Success,
            EmployeeNotFound,
            HasRelatedDevices
        }
}
