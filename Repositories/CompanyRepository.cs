using Contract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)=>
            await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(a => ids.Contains(a.Id), trackChanges).ToListAsync();

        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) =>
          await FindByCondition(c => c.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();

    }
}
