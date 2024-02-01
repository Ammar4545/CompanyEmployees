using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contract
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetEmployees(Guid companyId , bool trackChanges);
        EmployeeDto GetEmployee(Guid companyID, Guid id, bool trackChanges);
    }
}
