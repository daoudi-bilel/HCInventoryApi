using ITInventoryManagementAPI.Models;
using ITInventoryManagementAPI.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITInventoryManagementAPI.Services
{
    public interface IEmployeeService
    {
        Task<PagedResponse<EmployeeDto>> GetEmployeesAsync(int page = 1, int size = 10, string sortOrder = "ASC", string keyword = "");
        Task<SingleEmployeeDto> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(int id, EmployeeDto employee);
        Task<DeleteEmployeeResult> DeleteEmployeeAsync(int id);
        Task UpdateEmployeeDevicesAsync(int employeeId, List<int> deviceIds);
    }
        public enum DeleteEmployeeResult
        {
            Success,
            EmployeeNotFound,
            HasRelatedDevices
        }
}
