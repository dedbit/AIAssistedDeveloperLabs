using System;

namespace EmployeeManagement.Core.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Department { get; set; } = string.Empty;
        public decimal Salary { get; set; }
    }
}