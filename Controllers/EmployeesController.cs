using Microsoft.AspNetCore.Mvc;
using ITInventoryManagementAPI.Models;
using ITInventoryManagementAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using ITInventoryManagementAPI.Models.Responses;

namespace ITInventoryManagementAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all employees with pagination, optional keyword, and sorting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<Employee>>> GetEmployees(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string keyword = "",
            [FromQuery] string sortOrder = "ASC")
        {
            var pagedResponse = await _employeeService.GetEmployeesAsync(page, size, sortOrder, keyword);
            return Ok(pagedResponse);
        }
            
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new employee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get an employee by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with ID : {id} not found");
            }
            return employee;
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing employee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(id, employee);
            if (updatedEmployee == null)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an employee by ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            
            switch (result)
            {
                case DeleteEmployeeResult.EmployeeNotFound:
                    return NotFound(new { message = "Employee not found." });
                
                case DeleteEmployeeResult.HasRelatedDevices:
                    return Conflict(new { message = "Cannot delete employee. There are devices related to this employee. Please delete the devices first." });
                
                case DeleteEmployeeResult.Success:
                    return NoContent();
                
                default:
                    throw new InvalidOperationException("Unexpected result from DeleteEmployeeAsync");
            }
        }


        [HttpGet("search")]
        [SwaggerOperation(Summary = "Search employees by name or email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployees(string searchTerm)
        {
            var employees = await _employeeService.SearchEmployeesByNameOrEmailAsync(searchTerm);
            return Ok(employees);
        }
    }
}
