using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Yuu.Eip.HumanResources.Salaries;
using Yuu.Eip.Permissions;

namespace Yuu.Eip.HumanResources;

[Authorize(EipPermissions.HumanResources.Salaries.Default)]
public class SalaryAppService : EipAppService, ISalaryAppService
{
    private readonly IRepository<SalaryRecord, Guid> _salaryRepository;
    private readonly IRepository<Employee, Guid> _employeeRepository;
    private readonly IRepository<Position, Guid> _positionRepository;
    private readonly IRepository<Department, Guid> _departmentRepository;
    private readonly IRepository<AttendanceRecord, Guid> _attendanceRepository;
    private readonly IRepository<LeaveRequest, Guid> _leaveRequestRepository;
    private readonly HumanResourcesMapper _mapper;

    public SalaryAppService(
        IRepository<SalaryRecord, Guid> salaryRepository,
        IRepository<Employee, Guid> employeeRepository,
        IRepository<Position, Guid> positionRepository,
        IRepository<Department, Guid> departmentRepository,
        IRepository<AttendanceRecord, Guid> attendanceRepository,
        IRepository<LeaveRequest, Guid> leaveRequestRepository)
    {
        _salaryRepository = salaryRepository;
        _employeeRepository = employeeRepository;
        _positionRepository = positionRepository;
        _departmentRepository = departmentRepository;
        _attendanceRepository = attendanceRepository;
        _leaveRequestRepository = leaveRequestRepository;
        _mapper = new HumanResourcesMapper();
    }

    public async Task<SalaryRecordDto> GetAsync(Guid id)
    {
        var record = await _salaryRepository.GetAsync(id);
        var dto = _mapper.SalaryRecordToDto(record);
        await FillSalaryNamesAsync(new List<SalaryRecordDto> { dto });
        return dto;
    }

    [Authorize(EipPermissions.HumanResources.Salaries.ViewAll)]
    public async Task<PagedResultDto<SalaryRecordDto>> GetListAsync(GetSalaryRecordsInput input)
    {
        var query = await _salaryRepository.GetQueryableAsync();

        if (input.EmployeeId.HasValue)
        {
            query = query.Where(x => x.EmployeeId == input.EmployeeId);
        }

        if (input.DepartmentId.HasValue)
        {
            var employeeIds = (await _employeeRepository.GetQueryableAsync())
                .Where(x => x.DepartmentId == input.DepartmentId)
                .Select(x => x.Id)
                .ToList();
            query = query.Where(x => employeeIds.Contains(x.EmployeeId));
        }

        if (input.Year.HasValue)
        {
            query = query.Where(x => x.Year == input.Year);
        }

        if (input.Month.HasValue)
        {
            query = query.Where(x => x.Month == input.Month);
        }

        if (input.Status.HasValue)
        {
            query = query.Where(x => x.Status == input.Status);
        }

        var totalCount = query.Count();

        query = query.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month);

        var items = query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        var dtos = items.Select(_mapper.SalaryRecordToDto).ToList();
        await FillSalaryNamesAsync(dtos);

        return new PagedResultDto<SalaryRecordDto>(totalCount, dtos);
    }

    [Authorize(EipPermissions.HumanResources.Salaries.Calculate)]
    public async Task<SalaryRecordDto> CalculateAsync(CalculateSalaryInput input)
    {
        var employee = await _employeeRepository.GetAsync(input.EmployeeId);
        var position = await _positionRepository.GetAsync(employee.PositionId);

        /* Check if record already exists */
        var existingRecord = await _salaryRepository.FirstOrDefaultAsync(
            x => x.EmployeeId == input.EmployeeId && x.Year == input.Year && x.Month == input.Month);

        if (existingRecord != null && existingRecord.Status != SalaryStatus.Pending)
        {
            throw new BusinessException("HumanResources:SalaryAlreadyConfirmed");
        }

        /* Get attendance records for the month */
        var startDate = new DateTime(input.Year, input.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var attendances = (await _attendanceRepository.GetQueryableAsync())
            .Where(x => x.EmployeeId == input.EmployeeId && x.Date >= startDate && x.Date <= endDate)
            .ToList();

        /* Get approved leave requests for the month */
        var leaves = (await _leaveRequestRepository.GetQueryableAsync())
            .Where(x => x.EmployeeId == input.EmployeeId
                && x.Status == LeaveRequestStatus.Approved
                && x.StartDate <= endDate && x.EndDate >= startDate)
            .ToList();

        /* Calculate */
        var baseSalary = position.BaseSalary;
        var overtimeHours = attendances.Sum(x => x.OvertimeHours);
        var overtimePay = CalculateOvertimePay(baseSalary, overtimeHours);
        var deductions = CalculateDeductions(attendances, leaves, baseSalary);

        SalaryRecord record;
        if (existingRecord != null)
        {
            record = existingRecord;
        }
        else
        {
            record = new SalaryRecord(
                GuidGenerator.Create(),
                input.EmployeeId,
                input.Year,
                input.Month,
                CurrentTenant.Id);
        }

        record.Calculate(baseSalary, overtimePay, input.Bonus, deductions);

        if (existingRecord != null)
        {
            await _salaryRepository.UpdateAsync(record);
        }
        else
        {
            await _salaryRepository.InsertAsync(record);
        }

        var dto = _mapper.SalaryRecordToDto(record);
        await FillSalaryNamesAsync(new List<SalaryRecordDto> { dto });
        return dto;
    }

    [Authorize(EipPermissions.HumanResources.Salaries.Calculate)]
    public async Task<List<SalaryRecordDto>> BatchCalculateAsync(int year, int month, Guid? departmentId = null)
    {
        var employeeQuery = await _employeeRepository.GetQueryableAsync();
        employeeQuery = employeeQuery.Where(x => x.Status == EmployeeStatus.Active);

        if (departmentId.HasValue)
        {
            employeeQuery = employeeQuery.Where(x => x.DepartmentId == departmentId);
        }

        var employees = employeeQuery.ToList();
        var results = new List<SalaryRecordDto>();

        foreach (var employee in employees)
        {
            try
            {
                var result = await CalculateAsync(new CalculateSalaryInput
                {
                    EmployeeId = employee.Id,
                    Year = year,
                    Month = month,
                    Bonus = 0
                });
                results.Add(result);
            }
            catch
            {
                /* Skip employees with calculation errors */
            }
        }

        return results;
    }

    [Authorize(EipPermissions.HumanResources.Salaries.Confirm)]
    public async Task<SalaryRecordDto> ConfirmAsync(Guid id)
    {
        var record = await _salaryRepository.GetAsync(id);

        if (record.Status != SalaryStatus.Pending)
        {
            throw new BusinessException("HumanResources:SalaryNotPending");
        }

        record.Confirm();
        await _salaryRepository.UpdateAsync(record);

        var dto = _mapper.SalaryRecordToDto(record);
        await FillSalaryNamesAsync(new List<SalaryRecordDto> { dto });
        return dto;
    }

    [Authorize(EipPermissions.HumanResources.Salaries.Confirm)]
    public async Task<SalaryRecordDto> MarkAsPaidAsync(Guid id)
    {
        var record = await _salaryRepository.GetAsync(id);

        if (record.Status != SalaryStatus.Confirmed)
        {
            throw new BusinessException("HumanResources:SalaryNotConfirmed");
        }

        record.MarkAsPaid();
        await _salaryRepository.UpdateAsync(record);

        var dto = _mapper.SalaryRecordToDto(record);
        await FillSalaryNamesAsync(new List<SalaryRecordDto> { dto });
        return dto;
    }

    public async Task<SalarySlipDto> GetSalarySlipAsync(Guid id)
    {
        var record = await _salaryRepository.GetAsync(id);
        var employee = await _employeeRepository.GetAsync(record.EmployeeId);
        var department = await _departmentRepository.GetAsync(employee.DepartmentId);
        var position = await _positionRepository.GetAsync(employee.PositionId);

        var startDate = new DateTime(record.Year, record.Month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var attendances = (await _attendanceRepository.GetQueryableAsync())
            .Where(x => x.EmployeeId == employee.Id && x.Date >= startDate && x.Date <= endDate)
            .ToList();

        return new SalarySlipDto
        {
            EmployeeId = employee.Id,
            EmployeeName = employee.Name,
            EmployeeNo = employee.EmployeeNo,
            DepartmentName = department.Name,
            PositionName = position.Name,
            Year = record.Year,
            Month = record.Month,
            BaseSalary = record.BaseSalary,
            OvertimePay = record.OvertimePay,
            Bonus = record.Bonus,
            Deductions = record.Deductions,
            LaborInsurance = record.LaborInsurance,
            HealthInsurance = record.HealthInsurance,
            NetSalary = record.NetSalary,
            WorkDays = attendances.Count(x => x.ClockIn.HasValue),
            TotalWorkHours = attendances.Sum(x => x.WorkHours),
            TotalOvertimeHours = attendances.Sum(x => x.OvertimeHours),
            LateDays = attendances.Count(x => x.Status == AttendanceStatus.Late),
            LeaveDays = attendances.Count(x => x.Status == AttendanceStatus.OnLeave)
        };
    }

    public async Task<SalarySlipDto?> GetMySalarySlipAsync(int year, int month)
    {
        if (CurrentUser.Id == null)
        {
            throw new BusinessException("HumanResources:UserNotLoggedIn");
        }

        var employee = await _employeeRepository.FirstOrDefaultAsync(x => x.UserId == CurrentUser.Id) ?? throw new BusinessException("HumanResources:EmployeeNotFound");

        var record = await _salaryRepository.FirstOrDefaultAsync(
            x => x.EmployeeId == employee.Id && x.Year == year && x.Month == month);

        if (record == null)
        {
            return null;
        }

        return await GetSalarySlipAsync(record.Id);
    }

    private decimal CalculateOvertimePay(decimal baseSalary, decimal overtimeHours)
    {
        /* Overtime rate = Base Salary / 240 hours * 1.33 */
        var hourlyRate = baseSalary / HumanResourcesConsts.StandardMonthlyWorkHours;
        return Math.Round(overtimeHours * hourlyRate * HumanResourcesConsts.OvertimeRate, 0);
    }

    private decimal CalculateDeductions(
        List<AttendanceRecord> attendances,
        List<LeaveRequest> leaves,
        decimal baseSalary)
    {
        var deductions = 0m;
        var dailyRate = baseSalary / 30m;
        var hourlyRate = baseSalary / HumanResourcesConsts.StandardMonthlyWorkHours;

        /* Late deductions (30 min late = 0.5 hour deduction) */
        var lateDays = attendances.Count(x => x.Status == AttendanceStatus.Late);
        deductions += lateDays * hourlyRate * 0.5m;

        /* Absent deductions (full day) */
        var absentDays = attendances.Count(x => x.Status == AttendanceStatus.Absent);
        deductions += absentDays * dailyRate;

        /* Unpaid leave deductions */
        var unpaidLeaveHours = leaves
            .Where(x => x.LeaveType == LeaveType.Unpaid)
            .Sum(x => x.Hours);
        deductions += unpaidLeaveHours * hourlyRate;

        return Math.Round(deductions, 0);
    }

    private async Task FillSalaryNamesAsync(List<SalaryRecordDto> dtos)
    {
        var employeeIds = dtos.Select(x => x.EmployeeId).Distinct().ToList();
        var employees = await _employeeRepository.GetListAsync(x => employeeIds.Contains(x.Id));

        foreach (var dto in dtos)
        {
            var employee = employees.FirstOrDefault(x => x.Id == dto.EmployeeId);
            dto.EmployeeName = employee?.Name;
            dto.EmployeeNo = employee?.EmployeeNo;
        }
    }
}
