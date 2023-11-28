using Microsoft.AspNetCore.Mvc;
using SimpleApiProject.Models;
using SimpleApiProject.Services;

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
        private readonly ICompanyService companyService;
        private readonly IEmployeeService employeeService;

        public CompanyController(ILogger<CompanyController> logger, ICompanyService companyService, IEmployeeService employeeService)
        {
            this.logger = logger;
            this.companyService = companyService;
            this.employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IEnumerable<CompanyHeaderDto>> GetAll() =>
            (await companyService.FindMany()).ToCompanyHeaderDto();

        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetCompany(int companyId)
        {
            var company = await companyService.Find(companyId);

            if (company is null)
            {
                return NotFound();
            }

            return Ok(company.ToCompanyDto());
        }

        [HttpGet("{companyId}/Employees/{employeeNumber}")]
        public async Task<IActionResult> GetEmployee(int companyId, string employeeNumber)
        {
            var employee = await employeeService.Find(companyId, employeeNumber);

            if (employee is null)
            {
                return NotFound();
            }

            return Ok(employee.ToEmployeeDto());
        }
    }
}