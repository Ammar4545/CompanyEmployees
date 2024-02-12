using AutoMapper;
using Contract;
using Entities.Exceptions;
using Entities.Models;
using Service.Contract;
using Shared.DTOs;
using Shared.DTOs.Incoming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, false);
            if (company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.Save();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }

        public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, false);
            if (company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employee = _repository.Employee.GetEmployee(companyId,id, false);
            if (employee is null)
            {
                throw new EmployeeNotFoundException(id);
            }

            _repository.Employee.DeleteEmployee(employee);
            _repository.Save();
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company is null)
            {
                throw new EmployeeNotFoundException(id );
            }

            var employeeDb = _repository.Employee.GetEmployee(companyId,id, trackChanges: false);
            if (employeeDb is null)
            {
                throw new EmployeeNotFoundException(id);
            }

            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges: false);
            if (company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeesfromDb = _repository.Employee.GetEmployees(companyId, trackChanges: false);

            var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesfromDb);

            return employeeDto;
        }

        public void UpdateEmployeeForCompany
            (Guid companyId, Guid id, EmployeeForUpdateDto empForUpdate,
            bool compTrackChanges, bool empTrackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, compTrackChanges);
            if (company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employee = _repository.Employee.GetEmployee(companyId, id, empTrackChanges);
            if (company is null)
            {
                throw new EmployeeNotFoundException(id);
            }

            _mapper.Map(empForUpdate, employee );
            _repository.Save();
        }
    }
}
