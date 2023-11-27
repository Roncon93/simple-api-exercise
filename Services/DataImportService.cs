using CsvHelper;
using System.Globalization;

namespace SimpleApiProject.Services
{
    public interface IDataImportService
    {
        void Import();
    }

    public class DataImportService : IDataImportService
    {
        private readonly Dictionary<string, DataImportRow> rows = new Dictionary<string, DataImportRow>
        {
            { 
                "5E196582",
                new()
                {
                    Company = new()
                    {
                        CompanyId = 5,
                    },
                    Employee = new()
                    {
                        EmployeeNumber = "E196582",
                        ManagerEmployeeNumber = "E196581"
                    }
                }
            },
            {
                "5E196581",
                new()
                {
                    Company = new()
                    {
                        CompanyId = 5,
                    },
                    Employee = new()
                    {
                        EmployeeNumber = "E196581"
                    }
                }
            },
            {
                "5E196582",
                new()
                {
                    Company = new()
                    {
                        CompanyId = 5,
                    },
                    Employee = new()
                    {
                        EmployeeNumber = "E196582",
                        ManagerEmployeeNumber = "E196581"
                    }
                }
            }
        };
        private readonly HashSet<string> rowsProcessed = [];

        public async void Import(IFormFile file, CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);

            using TextReader streamReader = new StreamReader(stream);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var companies = new Dictionary<int, DataImportCompany>();
            var validEmployees = new List<DataImportEmployee>();

            foreach (var row in rows)
            {
                Validate(row.Value.Company, row.Value.Employee);

                if (!companies.ContainsKey(row.Value.Company.CompanyId))
                {
                    companies.Add(row.Value.Company.CompanyId, row.Value.Company);
                }

                if ((row.Value.Employee.IsValid ?? false))
                {
                    validEmployees.Add(row.Value.Employee);
                }
            }

            // Store companies
            // Store employees
        }

        private bool Validate(DataImportCompany company, DataImportEmployee employee)
        {
            var rowId = BuildDataImportRowId(company.CompanyId, employee.EmployeeNumber);

            bool isValid;

            if (employee.IsValid is not null)
            {
                return employee.IsValid.Value;
            }

            else if (rowsProcessed.Contains(rowId))
            {
                isValid = false;
            }

            else if (string.IsNullOrWhiteSpace(employee.ManagerEmployeeNumber))
            {
                isValid = true;
            }

            else if (employee.EmployeeNumber == employee.ManagerEmployeeNumber)
            {
                isValid = false;
            }

            else
            {
                rows.TryGetValue(BuildDataImportRowId(company.CompanyId, employee.ManagerEmployeeNumber), out var managerInfo);

                if (managerInfo is null)
                {
                    isValid = false;
                }

                else
                {
                    isValid = Validate(company, managerInfo.Employee);
                }
            }

            employee.IsValid = isValid;
            rowsProcessed.Add(rowId);

            return isValid;
        }

        private static string BuildDataImportRowId(int companyId, string employeeNumber) =>
            $"{companyId}{employeeNumber}";
    }

    public class DataImportRow
    {
        public DataImportCompany Company { get; set; } = new();

        public DataImportEmployee Employee { get; set; } = new();
    }

    public class DataImportCompany
    {
        public int CompanyId { get; set; }
    }

    public class DataImportEmployee
    {
        public string EmployeeNumber { get; set; } = string.Empty;

        public string ManagerEmployeeNumber {  get; set; } = string.Empty;

        public bool? IsValid { get; set; }
    }
}
