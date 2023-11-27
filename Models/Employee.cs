namespace SimpleApiProject.Models
{
    public class Employee
    {
        public string EmployeeNumber { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public IEnumerable<Employee> Managers { get; set; } = Enumerable.Empty<Employee>();

        public Company? Company { get; set; }
    }
}
