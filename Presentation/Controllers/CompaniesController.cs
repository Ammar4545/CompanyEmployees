using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Presentation.ModelBinders;
using Service.Contract;
using Shared.DTOs.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/Companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompaniesController(IServiceManager service) => _service = service;

        [HttpGet]
        public IActionResult GetCompanies()
        {

            var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
            return Ok(companies);
        }

        [HttpGet("{id:guid}", Name = "CompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _service.CompanyService.GetCompany(id, trackChanges: false);
            return Ok(company);
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if (company is null)
            {
                return BadRequest("CompanyForCreationDto is null");
            }

            var createdCompany = _service.CompanyService.CreateCompany(company);

            return CreatedAtRoute("CompanyById", new { Id = createdCompany.Id }, createdCompany);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto>? companyCollection)
        {
            var result = _service.CompanyService.CreateCompanyCollection(companyCollection);
            var test = result.ids;
            return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
        }

        [HttpGet("collections/({ids})", Name = "CompanyCollection")]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = _service.CompanyService.GetByIds(ids, trackChanges: false);
            return Ok(companies);
        }
    }
}