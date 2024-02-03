using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using Shared.DTOs.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    //[Route("api/Employees")]
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public EmployeesController(IServiceManager service) => _service = service;

        [HttpGet("{id:guid}", Name= "GetEmployeeForCompany")]
        public IActionResult GetEmployeesForCompany(Guid Companyid, Guid id)
        {
            var employee = _service.EmployeeService.GetEmployee(Companyid,id, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid CompanyId, [FromBody]EmployeeForCreationDto employee)
        {
            if (employee is null)
            {
                return BadRequest("EmployeeForCreationDto is null");
            }

            var employeeToReturn = _service.EmployeeService.CreateEmployeeForCompany(CompanyId, employee, false);

            return CreatedAtRoute("GetEmployeeForCompany", new {CompanyId,id = employeeToReturn.Id}, employeeToReturn);
        }
    }
}
