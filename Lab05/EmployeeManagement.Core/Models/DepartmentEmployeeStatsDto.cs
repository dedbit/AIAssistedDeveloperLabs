namespace EmployeeManagement.Core.Models
{
    public class DepartmentEmployeeStatsDto
    {
        public string Department { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
        public decimal AverageSalary { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal MaxSalary { get; set; }

        public string TopEarnerFirstName { get; set; } = string.Empty;
        public string TopEarnerLastName { get; set; } = string.Empty;
        public string TopEarnerEmail { get; set; } = string.Empty;
    }
}