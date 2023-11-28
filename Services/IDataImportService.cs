namespace SimpleApiProject.Services
{
    /// <summary>
    /// The interface of the service in charge of importing CSV
    /// company and employee information.
    /// </summary>
    public interface IDataImportService
    {
        /// <summary>
        /// Imports a CSV file containing company and employee information
        /// and stores it in a data store.
        /// </summary>
        /// <param name="file">The CSV file content.</param>
        /// <param name="cancellationToken">The operation's cancellation token.</param>
        /// <returns>The list of import errors.</returns>
        Task<IEnumerable<string>> Import(IFormFile file, CancellationToken cancellationToken = default);
    }
}
