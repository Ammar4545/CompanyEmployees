using Entities.Exceptions;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> GetEmployeeForCompany(Guid Companyid, Guid id)
        {
            var employee =await _service.EmployeeService.GetEmployeeAsync(Companyid,id, trackChanges: false);
            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId)
        {
            var employees =await _service.EmployeeService.GetEmployeesAsync(companyId, trackChanges: false);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid CompanyId, [FromBody]EmployeeForCreationDto employee)
        {
            if (employee is null)
                return BadRequest("EmployeeForCreationDto is null");
            

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);
            

            var employeeToReturn =await _service.EmployeeService.CreateEmployeeForCompanyAsync(CompanyId, employee, false);

            return CreatedAtRoute("GetEmployeeForCompany", new {CompanyId,id = employeeToReturn.Id}, employeeToReturn);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            await _service.EmployeeService.DeleteEmployeeForCompanyAsync(companyId, id, false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeeForCompany
            (Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employeeForUpdateDto)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.EmployeeService.UpdateEmployeeForCompanyAsync
                (companyId, id, employeeForUpdateDto, compTrackChanges: false, empTrackChanges: true);

            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany
            (Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null) return 
                BadRequest("patchDoc object sent from client is null.");

            var result =await _service.EmployeeService.GetEmployeeForPatch(companyId, id, false, true);

            patchDoc.ApplyTo(result.employeeToPatch, ModelState);

            TryValidateModel(result.employeeToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);

            return NoContent();
        }
    }
}
