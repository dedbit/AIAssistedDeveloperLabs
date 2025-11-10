using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Core.Models;
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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _repository.GetAllAsync();
            return Ok(employees);
        }

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

        [HttpPost]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            var created = await _repository.AddAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = created.Id }, created);
        }

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

        /// <summary>
        /// Gets compensation analysis showing employee distribution across salary ranges by department.
        /// This endpoint demonstrates a complex EF Core query with grouping, filtering, and aggregations.
        /// Example: GET /api/employees/compensation-analysis?bucketSize=25000&minEmployeeCount=2
        /// - sundhed.dk
        /// </summary>
        /// <param name="bucketSize">The salary range bucket size in dollars, e.g., 25000 for $25K buckets</param>
        /// <param name="minEmployeeCount">Minimum employees required to include a result, e.g., 1, 2, 5</param>
        /// <returns>List of compensation analysis grouped by salary range and department</returns>
        [HttpGet("compensation-analysis")]
        [ProducesResponseType(typeof(IEnumerable<CompensationAnalysisDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CompensationAnalysisDto>>> GetCompensationAnalysis(
            [FromQuery] decimal bucketSize = 25000m,
            [FromQuery] int minEmployeeCount = 1)
        {
            IEnumerable<CompensationAnalysisDto> analysis = await _repository.GetCompensationAnalysisAsync(bucketSize, minEmployeeCount);
            return Ok(analysis);
        }
    }
}