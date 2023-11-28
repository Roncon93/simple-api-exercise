namespace SimpleApiProject.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
