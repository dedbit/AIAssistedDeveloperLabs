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
        /// Returns aggregated employee statistics per department using filtering, grouping, and joining.
        /// </summary>
        /// <param name="minSalary">Optional minimum salary filter (inclusive).</param>
        /// <param name="search">Optional search term applied to first/last name, email, and department.</param>
        /// <returns>Department-level statistics with top earner info.</returns>
        public async Task<IEnumerable<DepartmentEmployeeStatsDto>> GetDepartmentStatsAsync(decimal? minSalary = null, string? search = null)
        {
            IQueryable<Employee> baseQuery = _context.Employees.AsNoTracking();

            if (minSalary.HasValue)
            {
                baseQuery = baseQuery.Where(e => e.Salary >= minSalary.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();
                baseQuery = baseQuery.Where(e =>
                    e.FirstName.ToLower().Contains(term) ||
                    e.LastName.ToLower().Contains(term) ||
                    e.Email.ToLower().Contains(term) ||
                    e.Department.ToLower().Contains(term));
            }

            // Aggregates per department (server-evaluated)
            var aggregates = await baseQuery
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    EmployeeCount = g.Count(),
                    AverageSalary = g.Average(x => x.Salary),
                    TotalSalary = g.Sum(x => x.Salary),
                    MaxSalary = g.Max(x => x.Salary)
                })
                .ToListAsync();

            // Self-join to get top earner candidates per department (server-evaluated join)
            var topEarnerCandidates = await (
                from e in baseQuery
                join m in baseQuery
                    .GroupBy(x => x.Department)
                    .Select(g => new { Department = g.Key, MaxSalary = g.Max(x => x.Salary) })
                  on new { e.Department, e.Salary } equals new { m.Department, Salary = m.MaxSalary }
                select new { e.Department, e.FirstName, e.LastName, e.Email, e.Salary }
            ).ToListAsync();

            // Choose one top earner per department on the client (ties resolved deterministically)
            var topEarnerPerDept = topEarnerCandidates
                .GroupBy(x => x.Department)
                .Select(g => g
                    .OrderByDescending(x => x.Salary)
                    .ThenBy(x => x.LastName)
                    .ThenBy(x => x.FirstName)
                    .First())
                .ToDictionary(x => x.Department, x => x);

            // Combine aggregates with top earner details
            var result = aggregates
                .Select(a => new DepartmentEmployeeStatsDto
                {
                    Department = a.Department,
                    EmployeeCount = a.EmployeeCount,
                    AverageSalary = a.AverageSalary,
                    TotalSalary = a.TotalSalary,
                    MaxSalary = a.MaxSalary,
                    TopEarnerFirstName = topEarnerPerDept.ContainsKey(a.Department) ? topEarnerPerDept[a.Department].FirstName : string.Empty,
                    TopEarnerLastName = topEarnerPerDept.ContainsKey(a.Department) ? topEarnerPerDept[a.Department].LastName : string.Empty,
                    TopEarnerEmail = topEarnerPerDept.ContainsKey(a.Department) ? topEarnerPerDept[a.Department].Email : string.Empty
                })
                .OrderByDescending(x => x.EmployeeCount)
                .ThenBy(x => x.Department)
                .ToList();

            return result;
        }
    }
}