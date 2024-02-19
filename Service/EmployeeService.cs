﻿using AutoMapper;
using Contract;
using Entities.Exceptions;
using Entities.Models;
using Service.Contract;
using Shared.DTOs;
using Shared.DTOs.Incoming;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        public async Task <EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
        {
            var company =await _repository.Company.GetCompanyAsync(companyId, false);
            if (company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
             await _repository.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }

        public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company =await _repository.Company.GetCompanyAsync(companyId, false);
            if (company is null)
            {
                throw new CompanyNotFoundException(companyId);
            }
            
            var employee =await _repository.Employee.GetEmployeeAsync(companyId,id, false);
            if (employee is null)
            {
                throw new EmployeeNotFoundException(id);
            }

            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await CheckIfCompanyExists(companyId, trackChanges);

            var employeeDb = await GetEmployeeAndcheckIfExists(companyId, id, trackChanges);

            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
        {
            var company = await CheckIfCompanyExists(companyId, trackChanges);

            var employeesfromDb =await _repository.Employee.GetEmployeesAsync(companyId, trackChanges: false);

            var employeeDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesfromDb);

            return employeeDto;
        }

        public async Task UpdateEmployeeForCompanyAsync
            (Guid companyId, Guid id, EmployeeForUpdateDto empForUpdate,
            bool compTrackChanges, bool empTrackChanges)
        {
            var company = await CheckIfCompanyExists(companyId, compTrackChanges);

            var employee = await GetEmployeeAndcheckIfExists(companyId, id, empTrackChanges);

            _mapper.Map(empForUpdate, employee );
            await _repository.SaveAsync();
        }

        public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatch(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            var company = await CheckIfCompanyExists(companyId, compTrackChanges);

            var employeeEntity = await GetEmployeeAndcheckIfExists(companyId, id, empTrackChanges);

            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

            return (employeeToPatch, employeeEntity);
        }

        public async Task SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity); 
            await _repository.SaveAsync();
        }

        private async Task<Company> CheckIfCompanyExists(Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
            if (company is null)
            {
                throw new CompanyNotFoundException(id);
            }
            return company;
        }
        private async Task<Employee> GetEmployeeAndcheckIfExists(Guid companyId,Guid id , bool trackChanges)
        {
            var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);

            if (employeeDb is null) 
                throw new EmployeeNotFoundException(id);

            return employeeDb;
        }
    }
}
