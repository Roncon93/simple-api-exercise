namespace SimpleApiProject.Models
{
    /// <summary>
    /// The database entity representing an employee department.
    /// </summary>
    public class EmployeeDepartment
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Employee> Employees { get; set;} = new List<Employee>();
    }
}
