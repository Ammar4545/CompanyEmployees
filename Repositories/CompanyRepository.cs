using Contract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CompanyRepository : RepositoryBase<Company> , ICompanyRepository
    {
        private readonly RepositoryContext _context;

        public CompanyRepository(RepositoryContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)=>
            FindAll(trackChanges).OrderBy(c => c.Name).ToList();



        public Company GetCompany(Guid companyId, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefault();

    }
}
