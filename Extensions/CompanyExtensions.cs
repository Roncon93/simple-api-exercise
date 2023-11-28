namespace SimpleApiProject.Models
{
    public static class CompanyExtensions
    {
        public static IEnumerable<CompanyHeaderDto> ToCompanyHeaderDto(this IEnumerable<Company> companies) =>
            companies.Select(c => c.ToCompanyHeaderDto());

        public static CompanyHeaderDto ToCompanyHeaderDto(this Company company) =>
            new()
            {
                Id = company.Id,
                Code = company.Code,
                Description = company.Description,
                EmployeeCount = company.Employees.Count
            };

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
