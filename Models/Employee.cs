namespace SimpleApiProject.Models
{
    public class Employee
    {
        public string EmployeeNumber { get; set; } = string.Empty;

        public string ManagerEmployeeNumber {  get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string EmployeeFullName => $"{FirstName} {LastName}";

        public string Email { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public DateTime? HireDate { get; set; }

        public IEnumerable<Employee> Managers { get; set; } = new List<Employee>();

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; } = new();
    }
}
