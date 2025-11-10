namespace EmployeeManagement.Core.Models
{
    /// <summary>
    /// Represents compensation analysis for a specific salary range and department.
    /// Example: SalaryRangeLabel = "$50K-$75K", Department = "IT", EmployeeCount = 5, AverageSalary = 62500
    /// - sundhed.dk
    /// </summary>
    public class CompensationAnalysisDto
    {
        /// <summary>
        /// Gets or sets the salary range label, e.g., "$0-$50K", "$50K-$75K"
        /// </summary>
        public string SalaryRangeLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the minimum salary in the range, e.g., 0, 50000
        /// </summary>
        public decimal SalaryRangeMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum salary in the range, e.g., 50000, 75000
        /// </summary>
        public decimal SalaryRangeMax { get; set; }

        /// <summary>
        /// Gets or sets the department name, e.g., "IT", "HR", "Finance"
        /// </summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the count of employees in this salary range and department, e.g., 5, 10
        /// </summary>
        public int EmployeeCount { get; set; }

        /// <summary>
        /// Gets or sets the average salary for this group, e.g., 62500.00
        /// </summary>
        public decimal AverageSalary { get; set; }

        /// <summary>
        /// Gets or sets the percentage of total employees in department, e.g., 35.5
        /// </summary>
        public decimal PercentageOfDepartment { get; set; }
    }
}
