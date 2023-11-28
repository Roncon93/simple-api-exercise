namespace SimpleApiProject.Models
{
    public class DataImportRecord
    {
        public string Id => $"{CompanyId}{EmployeeNumber}";

        public string ManagerId => $"{CompanyId}{ManagerEmployeeNumber}";

        public string EmployeeFullName => $"{EmployeeFirstName} {EmployeeLastName}";

        public int? CompanyId { get; set; }

        public string CompanyCode { get; set; } = string.Empty;

        public string CompanyDescription { get; set; } = string.Empty;

        public string EmployeeNumber { get; set; } = string.Empty;

        public string EmployeeFirstName { get; set; } = string.Empty;

        public string EmployeeLastName { get; set; } = string.Empty;

        public string EmployeeEmail { get; set; } = string.Empty;

        public string EmployeeDepartment { get; set; } = string.Empty;

        public DateTime? HireDate { get; set; }

        public string ManagerEmployeeNumber { get; set; } = string.Empty;
    }

    public class DataImportCompany
    {
        public int CompanyId { get; set; }
    }

    public class DataImportEmployee
    {
        public string EmployeeNumber { get; set; } = string.Empty;

        public string ManagerEmployeeNumber { get; set; } = string.Empty;
    }
}
