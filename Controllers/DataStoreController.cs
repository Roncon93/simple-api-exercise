using Microsoft.AspNetCore.Mvc;
using SimpleApiProject.Services;

namespace SimpleApiProject.Controllers
{
    /// <summary>
    /// Allows to upload a new company and employee data store.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DataStoreController : ControllerBase
    {
        private readonly ILogger<DataStoreController> logger;
        private readonly IDataImportService dataImportService;

        public DataStoreController(ILogger<DataStoreController> logger, IDataImportService dataImportService)
        {
            this.logger = logger;
            this.dataImportService = dataImportService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var errors = await dataImportService.Import(file);

            if (errors.Any())
            {
                return Ok(new { Errors = errors });
            }

            return Ok();
        }
    }
}