using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Configuration;
using SimpleApiProject.Data;
using SimpleApiProject.Data.Sqlite.Contexts;
using SimpleApiProject.Data.Sqlite.Repositories;
using SimpleApiProject.Models;
using SimpleApiProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApplicationConfiguration>(builder.Configuration);

var sqliteConnectionString = builder.Configuration.GetSection("Sqlite").GetValue<string>("ConnectionString");

builder.Services.AddDbContextFactory<CompanySqliteDbContext>(
    options => options.UseSqlite(sqliteConnectionString));

builder.Services.AddDbContextFactory<EmployeeSqliteDbContext>(
    options => options.UseSqlite(sqliteConnectionString));

builder.Services.AddDbContextFactory<EmployeeDepartmentSqliteDbContext>(
    options => options.UseSqlite(sqliteConnectionString));

builder.Services.AddSingleton<IRepository<Company>, CompanyRepository>();
builder.Services.AddSingleton<IRepository<Employee>, EmployeeRepository>();
builder.Services.AddSingleton<IRepository<EmployeeDepartment>, EmployeeDepartmentRepository>();

builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IEmployeeDepartmentService, EmployeeDepartmentService>();
builder.Services.AddTransient<IDataImportService, DataImportService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
