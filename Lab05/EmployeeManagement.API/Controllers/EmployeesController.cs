using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private readonly IEmployeeRepository _repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets all employees
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _repository.GetAllAsync();
            return Ok(employees);
        }

        /// <summary>
        /// Gets an employee by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">The employee to create. Example: { "FirstName": "Jane", "LastName": "Smith", "Email": "jane.smith@example.com", "Department": "HR", "DateOfBirth": "1992-05-15T00:00:00", "Salary": 60000 }</param>
        /// <returns>The created employee.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            var created = await _repository.AddAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">The ID of the employee to update. Example: 1</param>
        /// <param name="employee">The employee details to update. Example: { "Id": 1, "FirstName": "John", "LastName": "Doe", "Email": "john.doe@example.com", "Department": "IT", "DateOfBirth": "1985-10-20T00:00:00", "Salary": 75000 }</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            await _repository.UpdateAsync(employee);
            return NoContent();
        }

        /// <summary>
        /// Deletes an employee
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}