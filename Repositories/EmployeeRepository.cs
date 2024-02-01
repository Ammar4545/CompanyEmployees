using Contract;
using Entities.Models;
using Repositories;
using System;
using System.Collections.Generic;
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

        public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges)=>
            FindByCondition(c => c.CompanyId.Equals(companyId)&& c.Id.Equals(id) , trackChanges).SingleOrDefault();
        public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
            FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges: false).
            OrderBy(e=>e.Name).ToList();
    }
}
