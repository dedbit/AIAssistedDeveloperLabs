using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Models;

namespace EmployeeManagement.Core.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);

        /// <summary>
        /// Gets compensation analysis showing employee distribution across salary ranges by department.
        /// Example: bucketSize = 25000, minEmployeeCount = 1
        /// - sundhed.dk
        /// </summary>
        /// <param name="bucketSize">The salary range bucket size in dollars, e.g., 25000 for $25K buckets</param>
        /// <param name="minEmployeeCount">Minimum employees required to include a result, e.g., 1, 2, 5</param>
        /// <returns>List of compensation analysis grouped by salary range and department</returns>
        Task<IEnumerable<CompensationAnalysisDto>> GetCompensationAnalysisAsync(decimal bucketSize = 25000m, int minEmployeeCount = 1);
    }
}