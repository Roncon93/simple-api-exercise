using SimpleApiProject.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityServicesServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the necessary classes to read and write
        /// to the data store.
        /// </summary>
        /// <param name="services">The service collection to register the classes to.</param>
        /// <returns>The service collection with the registered classes.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services
                .AddSingleton<ICompanyService, CompanyService>()
                .AddSingleton<IEmployeeService, EmployeeService>()
                .AddSingleton<IEmployeeDepartmentService, EmployeeDepartmentService>()
                .AddSingleton<IDataImportService, DataImportService>();
    }
}
