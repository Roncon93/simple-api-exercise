namespace SimpleApiProject.Models
{
    /// <summary>
    /// Represents the company public facing DTO with employee header information.
    /// </summary>
    public class CompanyDto : CompanyHeaderDto
    {
        public EmployeeHeaderDto[] Employees { get; set; } = Array.Empty<EmployeeHeaderDto>();
    }
}
