namespace SimpleApiProject.Models
{
    /// <summary>
    /// Extension methods for the <see cref="Employee"/> entity.
    /// </summary>
    public static class EmployeeExtensions
    {
        /// <summary>
        /// Converts a collection of <see cref="Employee"/>
        /// to a collection of <see cref="EmployeeHeaderDto"/>.
        /// </summary>
        /// <param name="employees">The collection of employees to convert.</param>
        /// <returns>The list of converted employees.</returns>
        public static IEnumerable<EmployeeHeaderDto> ToEmployeeHeaderDto(this IEnumerable<Employee> employees) =>
            employees.Select(e => new EmployeeHeaderDto()
            {
                EmployeeNumber = e.EmployeeNumber,
                FullName = e.FullName
            });

        /// <summary>
        /// Converts a <see cref="Employee"/> to a <see cref="EmployeeDto"/>.
        /// </summary>
        /// <param name="employee">The employee to convert.</param>
        /// <returns>The converted employee.</returns>
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
