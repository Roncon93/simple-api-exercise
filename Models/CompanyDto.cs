namespace SimpleApiProject.Models
{
    /// <summary>
    /// Represents the company with employee header information.
    /// </summary>
    public class CompanyDto : CompanyHeaderDto
    {
        public EmployeeHeaderDto[] Employees { get; set; } = Array.Empty<EmployeeHeaderDto>();
    }
}
