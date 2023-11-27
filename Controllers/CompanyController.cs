using Microsoft.AspNetCore.Mvc;
using SimpleApiProject.Models;

namespace SimpleApiProject.Controllers
{
    /// <summary>
    /// Retrieves company information.
    /// </summary>
    [ApiController]
    [Route("Companies")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> logger;

        public CompanyController(ILogger<CompanyController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<CompanyHeaderDto> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{companyId}")]
        public CompanyDto GetCompany(string companyId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{companyId}/Employees/{employeeNumber}")]
        public EmployeeDto GetEmployee(string companyId, string employeeNumber)
        {
            throw new NotImplementedException();
        }
    }
}