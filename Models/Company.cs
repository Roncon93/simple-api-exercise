namespace SimpleApiProject.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IEnumerable<Employee> Employees { get; set; } = Enumerable.Empty<Employee>();
    }
}
