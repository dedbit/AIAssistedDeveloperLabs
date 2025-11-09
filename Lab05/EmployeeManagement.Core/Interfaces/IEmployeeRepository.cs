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
        Task<IEnumerable<DepartmentEmployeeStatsDto>> GetDepartmentStatsAsync(decimal? minSalary = null, string? search = null);
    }
}