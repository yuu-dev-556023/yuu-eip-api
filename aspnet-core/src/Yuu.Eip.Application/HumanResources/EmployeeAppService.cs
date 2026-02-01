using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Yuu.Eip.HumanResources.Employees;
using Yuu.Eip.Permissions;

namespace Yuu.Eip.HumanResources;

[Authorize(EipPermissions.HumanResources.Employees.Default)]
public class EmployeeAppService : EipAppService, IEmployeeAppService
{
    private readonly IRepository<Employee, Guid> _employeeRepository;
    private readonly IRepository<Department, Guid> _departmentRepository;
    private readonly IRepository<Position, Guid> _positionRepository;
    private readonly HumanResourcesMapper _mapper;

    public EmployeeAppService(
        IRepository<Employee, Guid> employeeRepository,
        IRepository<Department, Guid> departmentRepository,
        IRepository<Position, Guid> positionRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _positionRepository = positionRepository;
        _mapper = new HumanResourcesMapper();
    }

    public async Task<EmployeeDto> GetAsync(Guid id)
    {
        var employee = await _employeeRepository.GetAsync(id);
        var dto = _mapper.EmployeeToDto(employee);
        await FillEmployeeNamesAsync(new List<EmployeeDto> { dto });
        return dto;
    }

    public async Task<PagedResultDto<EmployeeListDto>> GetListAsync(GetEmployeesInput input)
    {
        var query = await _employeeRepository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            query = query.Where(x =>
                x.EmployeeNo.Contains(input.Filter) ||
                x.Name.Contains(input.Filter) ||
                (x.Email != null && x.Email.Contains(input.Filter)));
        }

        if (input.DepartmentId.HasValue)
        {
            query = query.Where(x => x.DepartmentId == input.DepartmentId);
        }

        if (input.PositionId.HasValue)
        {
            query = query.Where(x => x.PositionId == input.PositionId);
        }

        if (input.Status.HasValue)
        {
            query = query.Where(x => x.Status == input.Status);
        }

        var totalCount = query.Count();

        query = query.OrderBy(x => x.EmployeeNo);

        var employees = query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        var dtos = employees.Select(_mapper.EmployeeToListDto).ToList();
        await FillEmployeeListNamesAsync(dtos);

        return new PagedResultDto<EmployeeListDto>(totalCount, dtos);
    }

    [Authorize(EipPermissions.HumanResources.Employees.Create)]
    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto input)
    {
        var employee = new Employee(
            GuidGenerator.Create(),
            input.EmployeeNo,
            input.Name,
            input.Gender,
            input.HireDate,
            input.DepartmentId,
            input.PositionId,
            input.Email,
            input.Phone,
            input.BirthDate,
            input.ManagerId,
            input.UserId,
            CurrentTenant.Id);

        await _employeeRepository.InsertAsync(employee);

        var dto = _mapper.EmployeeToDto(employee);
        await FillEmployeeNamesAsync(new List<EmployeeDto> { dto });
        return dto;
    }

    [Authorize(EipPermissions.HumanResources.Employees.Update)]
    public async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto input)
    {
        var employee = await _employeeRepository.GetAsync(id);

        employee.Name = input.Name;
        employee.Email = input.Email;
        employee.Phone = input.Phone;
        employee.BirthDate = input.BirthDate;
        employee.Gender = input.Gender;
        employee.TerminationDate = input.TerminationDate;
        employee.Status = input.Status;
        employee.DepartmentId = input.DepartmentId;
        employee.PositionId = input.PositionId;
        employee.ManagerId = input.ManagerId;
        employee.UserId = input.UserId;

        await _employeeRepository.UpdateAsync(employee);

        var dto = _mapper.EmployeeToDto(employee);
        await FillEmployeeNamesAsync(new List<EmployeeDto> { dto });
        return dto;
    }

    [Authorize(EipPermissions.HumanResources.Employees.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _employeeRepository.DeleteAsync(id);
    }

    public async Task<List<EmployeeListDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetListAsync(x => x.Status == EmployeeStatus.Active);
        var dtos = employees
            .OrderBy(x => x.EmployeeNo)
            .Select(_mapper.EmployeeToListDto)
            .ToList();

        await FillEmployeeListNamesAsync(dtos);
        return dtos;
    }

    public async Task<EmployeeDto?> GetByUserIdAsync(Guid userId)
    {
        var employee = await _employeeRepository.FirstOrDefaultAsync(x => x.UserId == userId);
        if (employee == null)
        {
            return null;
        }

        var dto = _mapper.EmployeeToDto(employee);
        await FillEmployeeNamesAsync(new List<EmployeeDto> { dto });
        return dto;
    }

    private async Task FillEmployeeNamesAsync(List<EmployeeDto> dtos)
    {
        var departmentIds = dtos.Select(x => x.DepartmentId).Distinct().ToList();
        var positionIds = dtos.Select(x => x.PositionId).Distinct().ToList();
        var managerIds = dtos.Where(x => x.ManagerId.HasValue).Select(x => x.ManagerId!.Value).Distinct().ToList();

        var departments = await _departmentRepository.GetListAsync(x => departmentIds.Contains(x.Id));
        var positions = await _positionRepository.GetListAsync(x => positionIds.Contains(x.Id));
        var managers = managerIds.Any()
            ? await _employeeRepository.GetListAsync(x => managerIds.Contains(x.Id))
            : new List<Employee>();

        foreach (var dto in dtos)
        {
            dto.DepartmentName = departments.FirstOrDefault(x => x.Id == dto.DepartmentId)?.Name;
            dto.PositionName = positions.FirstOrDefault(x => x.Id == dto.PositionId)?.Name;
            dto.ManagerName = dto.ManagerId.HasValue
                ? managers.FirstOrDefault(x => x.Id == dto.ManagerId)?.Name
                : null;
        }
    }

    private async Task FillEmployeeListNamesAsync(List<EmployeeListDto> dtos)
    {
        var departmentIds = dtos.Select(x => x.Id).Distinct().ToList();
        var employees = await _employeeRepository.GetListAsync(x => departmentIds.Contains(x.Id));

        var actualDepartmentIds = employees.Select(x => x.DepartmentId).Distinct().ToList();
        var positionIds = employees.Select(x => x.PositionId).Distinct().ToList();

        var departments = await _departmentRepository.GetListAsync(x => actualDepartmentIds.Contains(x.Id));
        var positions = await _positionRepository.GetListAsync(x => positionIds.Contains(x.Id));

        foreach (var dto in dtos)
        {
            var emp = employees.FirstOrDefault(x => x.Id == dto.Id);
            if (emp != null)
            {
                dto.DepartmentName = departments.FirstOrDefault(x => x.Id == emp.DepartmentId)?.Name;
                dto.PositionName = positions.FirstOrDefault(x => x.Id == emp.PositionId)?.Name;
            }
        }
    }
}
