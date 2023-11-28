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
        public IActionResult Upload(IFormFile file)
        {
            dataImportService.Import(file);

            return Ok();
        }
    }
}