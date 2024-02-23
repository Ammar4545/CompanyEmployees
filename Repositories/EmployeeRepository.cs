using Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Shared.RequestParameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        private readonly RepositoryContext _context;

        public EmployeeRepository(RepositoryContext context) : base(context)
        {
            _context = context;
        }

        public void CreateEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee) => Delete(employee);

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
            await FindByCondition(c => c.CompanyId.Equals(companyId) && c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges) {
            var employee=await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges: false)
            .OrderBy(e => e.Name)
            .ToListAsync();

            return PagedList<Employee>.ToPageList(employee, employeeParameters.PageSize, employeeParameters.PageNumber);
        }
    }
}
