using Contract;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        private readonly RepositoryContext _context;

        public CompanyRepository(RepositoryContext context) : base(context)
        {
            _context = context;
        }

        public void CreateCompany(Company company) =>
            Create(company);

        public void DeleteCompany(Company company)=>
            Delete(company);

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)=>
            FindAll(trackChanges).OrderBy(c => c.Name).ToList();

        public IEnumerable<Company> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
            FindByCondition(a => ids.Contains(a.Id), trackChanges);

        public Company GetCompany(Guid companyId, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefault();

    }
}
