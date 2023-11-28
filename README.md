# simple-api-exercise

This is a simple .NET 8 Web API project with a few of RESTful endpoints.

The objective of this program is to receive a CSV file via its DataStore endpoint. The program then parses and stores the data from the file into a Sqlite database file (note that the project is setup so that any other data store implementation can be easily swapped in). The Company endpoints then allow a client to query company and/or employee information.

## Assumptions Made (PLEASE READ)

The following assumptions were made during the creation of the project:

* Records in the CSV file cannot have a missing company ID or a missing employee number:
    * None of these records will not be stored
* Records where an employee number is present in both the `EmployeeNumber` and `ManagerEmployeeNumber` will be allowed to be stored
* Employees can appear before their managers in the file
    * This forced the design to first load all the data before storing anything in the data store
* Records are allowed to have an empty hire date
* Employee number and names are not considered sensitive information and will logged
* Any record with a validation error will not be stored (this includes employee records where its related manager record is missing or the manager record has a conflicting employee number in the company). In other words:
    * Employee = Valid, Manager = Valid => Both are stored
    * Employee = Invalid, Manager = Valid => Only manager record is stored
    * Employee = Valid, Manager = Invalid => Neither will be stored
    * Employee = Invalid, Manager = Invalid => Neither will be stored

Many of the design decisions in the project are based off of these assumptions.

## Validations

* Missing company ID or employee number
* 2 or more records with the same company ID and employee number
* Employee's manager is missing their record

## How to run

Compile using `dotnet` commands or any .Net 8 compatible IDE and run. The project already comes with an empty database file `database.db` that has all the tables.

In case the databse file is having issues, you can re-create it using EntityFramework Migration Tools. Use the following commands to install and use the migration tools using `dotnet` commands:

```
dotnet tool install --global dotnet-ef
dotnet ef migrations add Test --context EmployeeSqliteDbContext
dotnet ef database update --context EmployeeSqliteDbContext
```