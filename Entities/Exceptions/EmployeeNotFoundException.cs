using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class EmployeeNotFoundException : NotFoundException 
    {
        public EmployeeNotFoundException(Guid employeeid) : base($"the Employee with the {employeeid} not exists")
        {
        }
    }
}
