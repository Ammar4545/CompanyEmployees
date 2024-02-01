using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetEmployees(Guid companyID, bool trackChanges);
        Employee GetEmployee(Guid companyID, Guid id, bool trackChanges);
    }
}
