namespace SimpleApiProject.Models
{
    /// <summary>
    /// The database entity representing an employee.
    /// </summary>
    public class Employee
    {
        public string EmployeeNumber { get; set; } = string.Empty;

        public string ManagerEmployeeNumber {  get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; } = string.Empty;

        public DateTime? HireDate { get; set; }

        public IEnumerable<Employee> Managers { get; set; } = new List<Employee>();

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; } = new();

        public int DepartmentId { get; set; }

        public virtual EmployeeDepartment Department { get; set; } = new();
    }
}
