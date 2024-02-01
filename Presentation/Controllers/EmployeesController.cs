using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Service.Contract;
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

        [HttpGet("{id:guid}")]
        public IActionResult GetEmployeesForCompany(Guid Companyid, Guid id)
        {
            var employee = _service.EmployeeService.GetEmployee(Companyid,id, trackChanges: false);
            return Ok(employee);
        }

        
    }
}
