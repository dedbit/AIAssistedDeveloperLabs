using EmployeeManagement.API.Data.Configurations;
using EmployeeManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
    }
}