using Microsoft.AspNetCore.Mvc;

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

        public DataStoreController(ILogger<DataStoreController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Upload()
        {
            return Ok();
        }
    }
}