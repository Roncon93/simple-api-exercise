namespace SimpleApiProject.Models
{
    /// <summary>
    /// Represents the employee information.
    /// </summary>
    public class EmployeeDto : EmployeeHeaderDto
    {
        public string Email { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public DateTime? HireDate { get; set; }

        public EmployeeHeaderDto[] Managers { get; set; } = Array.Empty<EmployeeHeaderDto>();
    }
}
