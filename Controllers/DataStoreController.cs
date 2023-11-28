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
            try
            {
                if (file.Length == 0)
                {
                    return BadRequest($"File {file.FileName} cannot be empty");
                }

                var errors = await dataImportService.Import(file);

                if (errors.Any())
                {
                    return Ok(new { Errors = errors });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error was encountered while importing CSV file");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}