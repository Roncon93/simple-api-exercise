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

        /// <summary>
        /// Retrieves all the company entities.
        /// </summary>
        /// <returns>The list of companies in a <see cref="CompanyHeaderDto"/> view.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok((await companyService.FindAll()).ToCompanyHeaderDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error was encountered while retrieving companies");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves a single company by its ID.
        /// </summary>
        /// <param name="companyId">The ID of the company to look for.</param>
        /// <returns>200 OK if the company was found, 404 Not Found otherwise.</returns>
        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetCompany(int companyId)
        {
            try
            {
                var company = await companyService.Find(companyId);

                if (company is null)
                {
                    return NotFound();
                }

                return Ok(company.ToCompanyDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error was encountered while retrieving company");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves a single employee by its company ID and employee number.
        /// </summary>
        /// <param name="companyId">The ID of the company to look for.</param>
        /// <param name="employeeNumber">The number of the employee to look for.</param>
        /// <returns>200 OK if the employee was found, 404 Not Found otherwise.</returns>
        [HttpGet("{companyId}/Employees/{employeeNumber}")]
        public async Task<IActionResult> GetEmployee(int companyId, string employeeNumber)
        {
            try
            {
                var employee = await employeeService.Find(companyId, employeeNumber);

                if (employee is null)
                {
                    return NotFound();
                }

                return Ok(employee.ToEmployeeDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error was encountered while retrieving employee");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}