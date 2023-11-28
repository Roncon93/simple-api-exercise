namespace SimpleApiProject.Models
{
    public static class EmployeeExtensions
    {
        public static IEnumerable<EmployeeHeaderDto> ToEmployeeHeaderDto(this IEnumerable<Employee> employees) =>
            employees.Select(e => new EmployeeHeaderDto()
            {
                EmployeeNumber = e.EmployeeNumber,
                FullName = e.FullName
            });

        public static EmployeeDto ToEmployeeDto(this Employee employee) =>
            new()
            {
                EmployeeNumber = employee.EmployeeNumber,
                Department = employee.Department.Name,
                Email = employee.Email,
                FullName = employee.FullName,
                HireDate = employee.HireDate,
                Managers = employee.Managers.ToEmployeeHeaderDto().ToArray()
            };
    }
}
