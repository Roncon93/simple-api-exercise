﻿using CsvHelper;
using CsvHelper.Configuration;
using SimpleApiProject.Models;
using System.Globalization;

namespace SimpleApiProject.Services
{
    public interface IDataImportService
    {
        void Import(IFormFile file, CancellationToken cancellationToken = default);
    }

    public class DataImportService : IDataImportService
    {
        public async void Import(IFormFile file, CancellationToken cancellationToken = default)
        {
            var recordsProcessed = new Dictionary<string, bool>();
            var errors = new List<string>();

            var records = await LoadDataImportRecords(file, errors, cancellationToken);      

            var companies = new Dictionary<int, DataImportCompany>();
            var validEmployees = new List<DataImportEmployee>();

            foreach (var row in records)
            {
                ValidateManagerInfo(row.Value, records, recordsProcessed, errors);

                if (row.Value.CompanyId is not null && !companies.ContainsKey(row.Value.CompanyId.Value))
                {
                    companies[row.Value.CompanyId.Value] = new() { CompanyId = row.Value.CompanyId.Value };
                }

                if (recordsProcessed[row.Value.Id])
                {
                    validEmployees.Add(new() { EmployeeNumber = row.Value.EmployeeNumber });
                }
            }

            // Store companies
            // Store employees
        }

        private static async Task<Dictionary<string, DataImportRecord>> LoadDataImportRecords(
            IFormFile file,
            List<string> errors,
            CancellationToken cancellationToken = default)
        {
            var records = new Dictionary<string, DataImportRecord>();

            using var stream = new MemoryStream(new byte[file.Length]);
            await file.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;

            using StreamReader streamReader = new StreamReader(stream);
            using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture));

            while (await csvReader.ReadAsync())
            {
                var record = csvReader.GetRecord<DataImportRecord>();

                if (record is null)
                {
                    continue;
                }

                var isValid = ValidateProperties(record, errors) && ValidateExistingRecord(records, record, errors);

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

        private static bool ValidateProperties(DataImportRecord record, List<string> errors)
        {
            bool isValid = true;

            if (record.CompanyId is null)
            {
                isValid = false;
                errors.Add($"Company ID is missing for record with employee number {record.EmployeeNumber}");
            }

            else if (string.IsNullOrWhiteSpace(record.EmployeeNumber))
            {
                isValid = false;
                errors.Add($"Employee number is missing for employee {record.EmployeeFullName} and company ID {record.CompanyId}");
            }

            else if (record.EmployeeNumber == record.ManagerEmployeeNumber)
            {
                isValid = false;
                errors.Add($"Employee {record.EmployeeNumber} in company with ID {record.CompanyId} cannot be their own manager");
            }

            else if (record.HireDate is null)
            {
                isValid = false;
                errors.Add($"Employee {record.EmployeeNumber} in company with ID {record.CompanyId} cannot have an empty hire date");
            }

            return isValid;
        }

        private static bool ValidateExistingRecord(Dictionary<string, DataImportRecord> records, DataImportRecord record, List<string> errors)
        {
            if (records.TryGetValue(record.Id, out DataImportRecord? existingEmployeeRecord))
            {
                errors.Add($"Employees {existingEmployeeRecord.EmployeeFullName} & {record.EmployeeFullName} have the same employee # {record.EmployeeNumber} in company with ID {record.CompanyId}");

                return false;
            }

            return true;
        }

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

                    errors.Add($"Record missing for manager with employee number {record.ManagerEmployeeNumber} in company with ID {record.CompanyId!.Value}");
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
