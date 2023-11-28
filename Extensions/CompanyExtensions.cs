namespace SimpleApiProject.Models
{
    /// <summary>
    /// Extension methods for the <see cref="Company"/> entity.
    /// </summary>
    public static class CompanyExtensions
    {
        /// <summary>
        /// Converts a collection of <see cref="Company"/>
        /// to a collection of <see cref="CompanyHeaderDto"/>.
        /// </summary>
        /// <param name="companies">The collection of companies to convert.</param>
        /// <returns>The list of converted companies.</returns>
        public static IEnumerable<CompanyHeaderDto> ToCompanyHeaderDto(this IEnumerable<Company> companies) =>
            companies.Select(c => c.ToCompanyHeaderDto());

        /// <summary>
        /// Converts a <see cref="Company"/> to a <see cref="CompanyHeaderDto"/>.
        /// </summary>
        /// <param name="company">The company to convert.</param>
        /// <returns>The converted company.</returns>
        public static CompanyHeaderDto ToCompanyHeaderDto(this Company company) =>
            new()
            {
                Id = company.Id,
                Code = company.Code,
                Description = company.Description,
                EmployeeCount = company.Employees.Count
            };

        /// <summary>
        /// Converts a <see cref="Company"/> to a <see cref="CompanyDto"/>.
        /// </summary>
        /// <param name="company">The company to convert.</param>
        /// <returns>The converted company.</returns>
        public static CompanyDto ToCompanyDto(this Company company) =>
            new()
            {
                Id = company.Id,
                Code = company.Code,
                Description = company.Description,
                EmployeeCount = company.Employees.Count,
                Employees = company.Employees.ToEmployeeHeaderDto().ToArray()
            };
    }
}
