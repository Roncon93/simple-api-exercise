using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Data;
using SimpleApiProject.Data.Sqlite.Contexts;
using SimpleApiProject.Data.Sqlite.Repositories;
using SimpleApiProject.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SqliteRepositoriesServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the necessary classes to be able to use
        /// Sqlite data store repositories.
        /// </summary>
        /// <param name="services">The services collection to register the classes to.</param>
        /// <param name="sqliteConnectionString">The Sqlite database connection string.</param>
        /// <returns>The services collection with the registered classes.</returns>
        public static IServiceCollection AddSqliteRepositories(
            this IServiceCollection services,
            string sqliteConnectionString) =>
            services

            // Register DbContext factories
            .AddDbContextFactory<CompanySqliteDbContext>(
                options => options.UseSqlite(sqliteConnectionString))
            .AddDbContextFactory<EmployeeSqliteDbContext>(
                options => options.UseSqlite(sqliteConnectionString))
            .AddDbContextFactory<EmployeeDepartmentSqliteDbContext>(
                options => options.UseSqlite(sqliteConnectionString))

            // Register repositories
            .AddSingleton<IRepository<Company>, CompanyRepository>()
            .AddSingleton<IRepository<Employee>, EmployeeRepository>()
            .AddSingleton<IRepository<EmployeeDepartment>, EmployeeDepartmentRepository>();
    }
}
