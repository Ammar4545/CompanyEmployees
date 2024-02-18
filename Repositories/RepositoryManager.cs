using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<ICompanyRepository> _companyContext;
        private readonly Lazy<IEmployeeRepository> _employeeContext;
        
        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _companyContext = new Lazy<ICompanyRepository>(() => new CompanyRepository(context));
            _employeeContext = new Lazy<IEmployeeRepository>(()=> new EmployeeRepository(context));

        }
        public IEmployeeRepository Employee => _employeeContext.Value;

        public ICompanyRepository Company => _companyContext.Value;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
