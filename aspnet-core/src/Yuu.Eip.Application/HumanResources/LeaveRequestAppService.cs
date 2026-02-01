using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Yuu.Eip.HumanResources.LeaveRequests;
using Yuu.Eip.Permissions;

namespace Yuu.Eip.HumanResources;

[Authorize(EipPermissions.HumanResources.LeaveRequests.Default)]
public class LeaveRequestAppService : EipAppService, ILeaveRequestAppService
{
    private readonly IRepository<LeaveRequest, Guid> _leaveRequestRepository;
    private readonly IRepository<Employee, Guid> _employeeRepository;
    private readonly HumanResourcesMapper _mapper;

    public LeaveRequestAppService(
        IRepository<LeaveRequest, Guid> leaveRequestRepository,
        IRepository<Employee, Guid> employeeRepository)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _employeeRepository = employeeRepository;
        _mapper = new HumanResourcesMapper();
    }

    public async Task<LeaveRequestDto> GetAsync(Guid id)
    {
        var leaveRequest = await _leaveRequestRepository.GetAsync(id);
        var dto = _mapper.LeaveRequestToDto(leaveRequest);
        await FillLeaveRequestNamesAsync(new List<LeaveRequestDto> { dto });
        return dto;
    }

    public async Task<PagedResultDto<LeaveRequestDto>> GetListAsync(GetLeaveRequestsInput input)
    {
        var query = await _leaveRequestRepository.GetQueryableAsync();

        query = ApplyFilters(query, input);

        var totalCount = query.Count();

        query = query.OrderByDescending(x => x.CreationTime);

        var items = query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        var dtos = items.Select(_mapper.LeaveRequestToDto).ToList();
        await FillLeaveRequestNamesAsync(dtos);

        return new PagedResultDto<LeaveRequestDto>(totalCount, dtos);
    }

    [Authorize(EipPermissions.HumanResources.LeaveRequests.Create)]
    public async Task<LeaveRequestDto> CreateAsync(CreateLeaveRequestDto input)
    {
        var leaveRequest = new LeaveRequest(
            GuidGenerator.Create(),
            input.EmployeeId,
            input.LeaveType,
            input.StartDate,
            input.EndDate,
            input.Hours,
            input.Reason,
            CurrentTenant.Id);

        await _leaveRequestRepository.InsertAsync(leaveRequest);

        var dto = _mapper.LeaveRequestToDto(leaveRequest);
        await FillLeaveRequestNamesAsync(new List<LeaveRequestDto> { dto });
        return dto;
    }

    [Authorize(EipPermissions.HumanResources.LeaveRequests.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var leaveRequest = await _leaveRequestRepository.GetAsync(id);
        if (leaveRequest.Status != LeaveRequestStatus.Pending)
        {
            throw new BusinessException("HumanResources:CannotDeleteNonPendingLeaveRequest");
        }

        await _leaveRequestRepository.DeleteAsync(id);
    }

    [Authorize(EipPermissions.HumanResources.LeaveRequests.Approve)]
    public async Task<LeaveRequestDto> ApproveAsync(Guid id, ApproveRejectLeaveRequestDto input)
    {
        var leaveRequest = await _leaveRequestRepository.GetAsync(id);
        if (leaveRequest.Status != LeaveRequestStatus.Pending)
        {
            throw new BusinessException("HumanResources:LeaveRequestNotPending");
        }

        var currentEmployee = await GetCurrentEmployeeAsync();
        leaveRequest.Approve(currentEmployee.Id, input.Comment);

        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        var dto = _mapper.LeaveRequestToDto(leaveRequest);
        await FillLeaveRequestNamesAsync(new List<LeaveRequestDto> { dto });
        return dto;
    }

    [Authorize(EipPermissions.HumanResources.LeaveRequests.Approve)]
    public async Task<LeaveRequestDto> RejectAsync(Guid id, ApproveRejectLeaveRequestDto input)
    {
        var leaveRequest = await _leaveRequestRepository.GetAsync(id);
        if (leaveRequest.Status != LeaveRequestStatus.Pending)
        {
            throw new BusinessException("HumanResources:LeaveRequestNotPending");
        }

        var currentEmployee = await GetCurrentEmployeeAsync();
        leaveRequest.Reject(currentEmployee.Id, input.Comment);

        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        var dto = _mapper.LeaveRequestToDto(leaveRequest);
        await FillLeaveRequestNamesAsync(new List<LeaveRequestDto> { dto });
        return dto;
    }

    public async Task<LeaveRequestDto> CancelAsync(Guid id)
    {
        var leaveRequest = await _leaveRequestRepository.GetAsync(id);

        var currentEmployee = await GetCurrentEmployeeAsync();
        if (leaveRequest.EmployeeId != currentEmployee.Id)
        {
            throw new BusinessException("HumanResources:CannotCancelOthersLeaveRequest");
        }

        if (leaveRequest.Status != LeaveRequestStatus.Pending)
        {
            throw new BusinessException("HumanResources:CannotCancelNonPendingLeaveRequest");
        }

        leaveRequest.Cancel();
        await _leaveRequestRepository.UpdateAsync(leaveRequest);

        var dto = _mapper.LeaveRequestToDto(leaveRequest);
        await FillLeaveRequestNamesAsync(new List<LeaveRequestDto> { dto });
        return dto;
    }

    public async Task<PagedResultDto<LeaveRequestDto>> GetMyLeaveRequestsAsync(GetLeaveRequestsInput input)
    {
        var currentEmployee = await GetCurrentEmployeeAsync();
        input.EmployeeId = currentEmployee.Id;
        return await GetListAsync(input);
    }

    [Authorize(EipPermissions.HumanResources.LeaveRequests.Approve)]
    public async Task<PagedResultDto<LeaveRequestDto>> GetPendingApprovalsAsync(GetLeaveRequestsInput input)
    {
        input.Status = LeaveRequestStatus.Pending;
        return await GetListAsync(input);
    }

    private IQueryable<LeaveRequest> ApplyFilters(IQueryable<LeaveRequest> query, GetLeaveRequestsInput input)
    {
        if (input.EmployeeId.HasValue)
        {
            query = query.Where(x => x.EmployeeId == input.EmployeeId);
        }

        if (input.LeaveType.HasValue)
        {
            query = query.Where(x => x.LeaveType == input.LeaveType);
        }

        if (input.Status.HasValue)
        {
            query = query.Where(x => x.Status == input.Status);
        }

        if (input.StartDateFrom.HasValue)
        {
            query = query.Where(x => x.StartDate >= input.StartDateFrom);
        }

        if (input.StartDateTo.HasValue)
        {
            query = query.Where(x => x.StartDate <= input.StartDateTo);
        }

        return query;
    }

    private async Task<Employee> GetCurrentEmployeeAsync()
    {
        if (CurrentUser.Id == null)
        {
            throw new BusinessException("HumanResources:UserNotLoggedIn");
        }

        var employee = await _employeeRepository.FirstOrDefaultAsync(x => x.UserId == CurrentUser.Id) ?? throw new BusinessException("HumanResources:EmployeeNotFound");

        return employee;
    }

    private async Task FillLeaveRequestNamesAsync(List<LeaveRequestDto> dtos)
    {
        var employeeIds = dtos.Select(x => x.EmployeeId).Distinct().ToList();
        var approverIds = dtos.Where(x => x.ApproverId.HasValue).Select(x => x.ApproverId!.Value).Distinct().ToList();

        var allIds = employeeIds.Union(approverIds).Distinct().ToList();
        var employees = await _employeeRepository.GetListAsync(x => allIds.Contains(x.Id));

        foreach (var dto in dtos)
        {
            var employee = employees.FirstOrDefault(x => x.Id == dto.EmployeeId);
            dto.EmployeeName = employee?.Name;
            dto.EmployeeNo = employee?.EmployeeNo;

            if (dto.ApproverId.HasValue)
            {
                dto.ApproverName = employees.FirstOrDefault(x => x.Id == dto.ApproverId)?.Name;
            }
        }
    }
}
