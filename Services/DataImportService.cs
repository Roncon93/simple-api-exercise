using CsvHelper;
using CsvHelper.Configuration;
using SimpleApiProject.Models;
using System.Globalization;

namespace SimpleApiProject.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly ICompanyService companyService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;

        public DataImportService(
            ICompanyService companyRepository,
            IEmployeeService employeeRepository,
            IEmployeeDepartmentService employeeDepartmentService)
        {
            this.companyService = companyRepository;
            this.employeeService = employeeRepository;
            this.employeeDepartmentService = employeeDepartmentService;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> Import(IFormFile file, CancellationToken cancellationToken = default)
        {
            var recordsProcessed = new Dictionary<string, bool>();
            var errors = new List<string>();

            var records = await LoadDataImportRecords(file, recordsProcessed, errors, cancellationToken);      

            if (!records.Any())
            {
                errors.Add($"No valid records were found in the file {file.FileName}");

                return errors;
            }

            var companies = new Dictionary<int, Company>();
            var departments = new Dictionary<string, EmployeeDepartment>();
            var validEmployees = new List<Employee>();

            var departmentsIdCounter = 0;

            foreach (var row in records)
            {
                // Invalidate employee records with missing manager records
                ValidateManagerInfo(row.Value, records, recordsProcessed, errors);

                // Add a companies that haven't been seen before
                if (row.Value.CompanyId is not null &&
                    !companies.ContainsKey(row.Value.CompanyId.Value))
                {
                    companies[row.Value.CompanyId.Value] = new()
                    {
                        Id = row.Value.CompanyId.Value,
                        Code = row.Value.CompanyCode,
                        Description = row.Value.CompanyDescription
                    };
                }

                // Add employee departments that haven't been seen before
                if (!string.IsNullOrWhiteSpace(row.Value.EmployeeDepartment) &&
                    !departments.ContainsKey(row.Value.EmployeeDepartment))
                {
                    departments[row.Value.EmployeeDepartment] = new()
                    {
                        Id = ++departmentsIdCounter,
                        Name = row.Value.EmployeeDepartment
                    };
                }

                // Only store employee records that are valid
                if (recordsProcessed[row.Value.Id])
                {
                    validEmployees.Add(new()
                    {
                        EmployeeNumber = row.Value.EmployeeNumber,
                        ManagerEmployeeNumber = row.Value.ManagerEmployeeNumber,
                        FirstName = row.Value.EmployeeFirstName,
                        LastName = row.Value.EmployeeLastName,
                        Email = row.Value.EmployeeEmail,
                        HireDate = row.Value.HireDate,
                        Company = companies[row.Value.CompanyId!.Value], // Creates missing company entities
                        Department = departments[row.Value.EmployeeDepartment] // Creates missing employee department entities
                    });
                }
            }

            // Remove all company and employee related entities
            await companyService.RemoveAll(cancellationToken);
            await employeeDepartmentService.RemoveAll(cancellationToken);
            await employeeService.RemoveAll(cancellationToken);

            // Insert all valid company, employee and employee department entities found
            // NOTE: The one-to-many relationship the employee entity has will also insert missing
            // company and employee department entities
            await employeeService.CreateMany(validEmployees, cancellationToken);

            return errors;
        }

        /// <summary>
        /// Loads CSV file records into a collection of <see cref="DataImportRecord"/>.
        /// 
        /// Note: This method will return only records that have the required properties present.
        /// Invalid records will be added to the list of error messages. This is a performance improvement
        /// as records are checked while they're being loaded for the first time without having to iterate
        /// over them again for required properties validations.
        /// </summary>
        /// <param name="file">The CSV file containing the company and employee information.</param>
        /// <param name="errors">The list of errors to add validation errors to.</param>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The collection of records without missing required properties..</returns>
        private static async Task<Dictionary<string, DataImportRecord>> LoadDataImportRecords(
            IFormFile file,
            Dictionary<string, bool> recordsProcessed,
            List<string> errors,
            CancellationToken cancellationToken = default)
        {
            var records = new Dictionary<string, DataImportRecord>();

            using var stream = new MemoryStream(new byte[file.Length]);
            await file.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;

            using StreamReader streamReader = new(stream);
            using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture));

            while (await csvReader.ReadAsync())
            {
                var record = csvReader.GetRecord<DataImportRecord>();

                if (record is null)
                {
                    continue;
                }

                var isValid = ValidateRequiredProperties(record, errors) && ValidateExistingRecord(records, record, recordsProcessed, errors);

                if (isValid)
                {
                    records[record.Id] = record;
                }

                else
                {
                    records.Remove(record.Id);
                }
            }

            return records;
        }

        /// <summary>
        /// Validates a record's required properties to make sure
        /// they are not missing.
        /// </summary>
        /// <param name="record">The record to validate.</param>
        /// <param name="errors">The list of errors to add validation messages to.</param>
        /// <returns>True if the record is valid, false otherwise.</returns>
        private static bool ValidateRequiredProperties(DataImportRecord record, List<string> errors)
        {
            bool isValid = true;

            if (record.CompanyId is null)
            {
                isValid = false;
                errors.Add($"Company ID is missing for record with employee number [{record.EmployeeNumber}]");
            }

            else if (string.IsNullOrWhiteSpace(record.EmployeeNumber))
            {
                isValid = false;
                errors.Add($"Employee # is missing for employee [{record.EmployeeFullName}] in company with ID [{record.CompanyId}]");
            }

            //else if (record.EmployeeNumber == record.ManagerEmployeeNumber)
            //{
            //    isValid = false;
            //    errors.Add($"Employee {record.EmployeeNumber} in company with ID {record.CompanyId} cannot be their own manager");
            //}

            //else if (record.HireDate is null)
            //{
            //    isValid = false;
            //    errors.Add($"Employee {record.EmployeeNumber} in company with ID {record.CompanyId} cannot have an empty hire date");
            //}

            return isValid;
        }

        /// <summary>
        /// Validates that records do not have the same
        /// key identifying properties.
        /// </summary>
        /// <param name="records">The collection of all records.</param>
        /// <param name="record">The record to validate.</param>
        /// <param name="errors">The list of erros to add validation errors to.</param>
        /// <returns>True if the record is unique, false otherwise.</returns>
        private static bool ValidateExistingRecord(
            Dictionary<string, DataImportRecord> records,
            DataImportRecord record,
            Dictionary<string, bool>  recordsProcessed,
            List<string> errors)
        {
            if (records.TryGetValue(record.Id, out DataImportRecord? existingEmployeeRecord))
            {
                recordsProcessed.TryAdd(record.Id, false);

                errors.Add($"Employees [{existingEmployeeRecord.EmployeeFullName}] & [{record.EmployeeFullName}] have the same employee # [{record.EmployeeNumber}] in company with ID [{record.CompanyId}]");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates that an employee record is not missing
        /// their manager's record infromation.
        /// </summary>
        /// <param name="record">The record to validate.</param>
        /// <param name="records">The collection of all the records (contains manager record information).</param>
        /// <param name="recordsProcessed">The collection of records that have already been validated.</param>
        /// <param name="errors">The list of errors to add validation messages to.</param>
        /// <returns>True if the record has manager record information, false otherwise.</returns>
        private static bool ValidateManagerInfo(
            DataImportRecord record,
            Dictionary<string, DataImportRecord> records,
            Dictionary<string, bool> recordsProcessed,
            List<string> errors)
        {
            bool isValid;

            if (recordsProcessed.TryGetValue(record.Id, out bool value))
            {
                isValid = value;
            }

            else if (string.IsNullOrWhiteSpace(record.ManagerEmployeeNumber))
            {
                isValid = true;
            }

            else
            {
                records.TryGetValue(record.ManagerId, out var managerInfo);

                if (managerInfo is null)
                {
                    isValid = false;

                    errors.Add($"Employee [{record.EmployeeNumber}] in company with ID [{record.CompanyId!.Value}] is missing manager record with employee # [{record.ManagerEmployeeNumber}]");
                }

                else
                {
                    isValid = ValidateManagerInfo(managerInfo, records, recordsProcessed, errors);
                }
            }

            recordsProcessed[record.Id] = isValid;

            return isValid;
        }
    }
}
