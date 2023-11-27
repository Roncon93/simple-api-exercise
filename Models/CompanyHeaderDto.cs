namespace SimpleApiProject.Models
{
    /// <summary>
    /// Represents the company surface information.
    /// </summary>
    public class CompanyHeaderDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int EmployeeCount { get; set; }
    }
}
