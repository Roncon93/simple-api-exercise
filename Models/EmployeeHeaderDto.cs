namespace SimpleApiProject.Models
{
    /// <summary>
    /// Represents the employee public facing DTO surface information.
    /// </summary>
    public class EmployeeHeaderDto
    {
        public string EmployeeNumber { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
    }
}
