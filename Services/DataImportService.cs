using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace SimpleApiProject.Services
{
    public interface IDataImportService
    {
        void Import(IFormFile file, CancellationToken cancellationToken = default);
    }

    public class DataImportService : IDataImportService
    {
        private readonly Dictionary<string, bool> rowsProcessed = [];
        private readonly List<string> errors = [];

        public async void Import(IFormFile file, CancellationToken cancellationToken = default)
        {
            var rows = await LoadDataImportRows(file, errors, cancellationToken);      

            var companies = new Dictionary<int, DataImportCompany>();
            var validEmployees = new List<DataImportEmployee>();

            foreach (var row in rows)
            {
                Validate(row.Value, rows);

                if (row.Value.CompanyId is not null && !companies.ContainsKey(row.Value.CompanyId.Value))
                {
                    companies[row.Value.CompanyId.Value] = new() { CompanyId = row.Value.CompanyId.Value };
                }

                if (rowsProcessed[row.Value.Id])
                {
                    validEmployees.Add(new() { EmployeeNumber = row.Value.EmployeeNumber });
                }
            }

            // Store companies
            // Store employees
        }

        private static async Task<Dictionary<string, DataImportRow>> LoadDataImportRows(
            IFormFile file,
            List<string> errors,
            CancellationToken cancellationToken = default)
        {
            var rows = new Dictionary<string, DataImportRow>();

            using var stream = new MemoryStream(new byte[file.Length]);
            await file.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;

            using StreamReader streamReader = new StreamReader(stream);
            using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture));

            while (await csvReader.ReadAsync())
            {
                var record = csvReader.GetRecord<DataImportRow>();

                if (record is null)
                {
                    continue;
                }

                if (record.CompanyId is null)
                {
                    errors.Add($"Company ID is missing for record with employee number {record.EmployeeNumber}");
                }

                else if (string.IsNullOrWhiteSpace(record.EmployeeNumber))
                {
                    errors.Add($"Employee number is missing for employee {record.EmployeeFullName} and company ID {record.CompanyId}");
                }

                else if (record.EmployeeNumber == record.ManagerEmployeeNumber)
                {
                    errors.Add($"Employee {record.EmployeeNumber} in company with ID {record.CompanyId} cannot be their own manager");
                }

                else if (record.HireDate is null)
                {
                    errors.Add($"Employee {record.EmployeeNumber} in company with ID {record.CompanyId} cannot have an empty hire date");
                }

                else if (rows.TryGetValue(record.Id, out DataImportRow? existingEmployeeRecord))
                {
                    rows.Remove(record.Id);

                    errors.Add($"Employees {existingEmployeeRecord.EmployeeFullName} & {record.EmployeeFullName} have the same employee # {record.EmployeeNumber} in company with ID {record.CompanyId}");
                }

                else
                {
                    rows[record.Id] = record;
                }
            }

            return rows;
        }

        private bool Validate(DataImportRow row, Dictionary<string, DataImportRow> rows)
        {
            bool isValid;

            if (rowsProcessed.TryGetValue(row.Id, out bool value))
            {
                isValid = value;
            }

            else if (string.IsNullOrWhiteSpace(row.ManagerEmployeeNumber))
            {
                isValid = true;
            }

            else
            {
                rows.TryGetValue(BuildDataImportRowId(row.CompanyId!.Value, row.ManagerEmployeeNumber), out var managerInfo);

                if (managerInfo is null)
                {
                    isValid = false;

                    errors.Add($"Record missing for manager with employee number {row.ManagerEmployeeNumber} in company with ID {row.CompanyId.Value}");
                }

                else
                {
                    isValid = Validate(managerInfo, rows);
                }
            }

            rowsProcessed[row.Id] = isValid;

            return isValid;
        }

        private static string BuildDataImportRowId(int companyId, string employeeNumber) =>
            $"{companyId}{employeeNumber}";
    }

    public class DataImportRow
    {
        public string Id => $"{CompanyId}{EmployeeNumber}";

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

        public string ManagerEmployeeNumber {  get; set; } = string.Empty;

        public bool? IsValid { get; set; }
    }
}
