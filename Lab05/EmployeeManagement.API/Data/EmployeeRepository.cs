using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using EmployeeManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Data
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            throw new Exception("Not implemented");
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets compensation analysis showing employee distribution across salary ranges by department.
        /// Uses complex EF Core query with grouping, filtering, joining, and salary bucketing.
        /// Example: bucketSize = 25000, minEmployeeCount = 1
        /// - sundhed.dk
        /// </summary>
        /// <param name="bucketSize">The salary range bucket size in dollars, e.g., 25000 for $25K buckets</param>
        /// <param name="minEmployeeCount">Minimum employees required to include a result, e.g., 1, 2, 5</param>
        /// <returns>List of compensation analysis grouped by salary range and department</returns>
        public async Task<IEnumerable<CompensationAnalysisDto>> GetCompensationAnalysisAsync(decimal bucketSize = 25000m, int minEmployeeCount = 1)
        {
            // Single LINQ query: group, bucket, aggregate, and calculate percentage in one step
            var query = from e in _context.Employees
                        let bucketMin = Math.Floor(e.Salary / bucketSize) * bucketSize
                        let bucketMax = (Math.Floor(e.Salary / bucketSize) + 1) * bucketSize
                        group e by new { bucketMin, bucketMax, e.Department } into g
                        let departmentTotal = _context.Employees.Count(emp => emp.Department == g.Key.Department)
                        let employeeCount = g.Count()
                        where employeeCount >= minEmployeeCount
                        orderby g.Key.Department, g.Key.bucketMin
                        select new CompensationAnalysisDto
                        {
                            SalaryRangeMin = g.Key.bucketMin,
                            SalaryRangeMax = g.Key.bucketMax,
                            SalaryRangeLabel = $"${g.Key.bucketMin / 1000}K-${g.Key.bucketMax / 1000}K",
                            Department = g.Key.Department,
                            EmployeeCount = employeeCount,
                            AverageSalary = g.Average(x => x.Salary),
                            PercentageOfDepartment = departmentTotal > 0
                                ? Math.Round((decimal)employeeCount / departmentTotal * 100, 2)
                                : 0
                        };

            return await query.ToListAsync();
        }
    }
}