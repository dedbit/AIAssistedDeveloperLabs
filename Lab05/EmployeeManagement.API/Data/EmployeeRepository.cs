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
            #pragma warning restore CS0162
        }

        
        public async Task<IEnumerable<CompensationAnalysisDto>> GetCompensationAnalysisAsync(decimal bucketSize = 25000m, int minEmployeeCount = 1)
        {
            var employees = await _context.Employees.ToListAsync();

            return employees
                .GroupBy(e => e.Department)
                .SelectMany(deptGroup =>
                {
                    var departmentTotal = deptGroup.Count();
                    return deptGroup
                        .GroupBy(e => new
                        {
                            BucketMin = Math.Floor(e.Salary / bucketSize) * bucketSize,
                            BucketMax = (Math.Floor(e.Salary / bucketSize) + 1) * bucketSize,
                            e.Department
                        })
                        .Where(bucketGroup => bucketGroup.Count() >= minEmployeeCount)
                        .Select(bucketGroup => new CompensationAnalysisDto
                        {
                            SalaryRangeMin = bucketGroup.Key.BucketMin,
                            SalaryRangeMax = bucketGroup.Key.BucketMax,
                            SalaryRangeLabel = $"${bucketGroup.Key.BucketMin / 1000}K-${bucketGroup.Key.BucketMax / 1000}K",
                            Department = bucketGroup.Key.Department,
                            EmployeeCount = bucketGroup.Count(),
                            AverageSalary = bucketGroup.Average(x => x.Salary),
                            PercentageOfDepartment = departmentTotal > 0
                                ? Math.Round((decimal)bucketGroup.Count() / departmentTotal * 100, 2)
                                : 0
                        });
                })
                .OrderBy(r => r.Department)
                .ThenBy(r => r.SalaryRangeMin)
                .ToList();
        }

        
    }
}