using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Yuu.Eip.HumanResources.Attendance;
using Yuu.Eip.Permissions;

namespace Yuu.Eip.HumanResources;

[Authorize(EipPermissions.HumanResources.Attendance.Default)]
public class AttendanceAppService : EipAppService, IAttendanceAppService
{
    private readonly IRepository<AttendanceRecord, Guid> _attendanceRepository;
    private readonly IRepository<Employee, Guid> _employeeRepository;
    private readonly IRepository<Department, Guid> _departmentRepository;
    private readonly HumanResourcesMapper _mapper;

    /* Default work hours (can be configured) */
    private readonly TimeSpan _workStartTime = new TimeSpan(9, 0, 0); // 09:00
    private readonly TimeSpan _workEndTime = new TimeSpan(18, 0, 0);  // 18:00

    /// <summary>
    /// 初始化出勤應用服務
    /// </summary>
    /// <param name="attendanceRepository">出勤記錄倉儲</param>
    /// <param name="employeeRepository">員工倉儲</param>
    /// <param name="departmentRepository">部門倉儲</param>
    public AttendanceAppService(
        IRepository<AttendanceRecord, Guid> attendanceRepository,
        IRepository<Employee, Guid> employeeRepository,
        IRepository<Department, Guid> departmentRepository)
    {
        _attendanceRepository = attendanceRepository;
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _mapper = new HumanResourcesMapper();
    }

    /// <summary>
    /// 獲取指定ID的出勤記錄
    /// </summary>
    /// <param name="id">出勤記錄ID</param>
    /// <returns>出勤記錄詳情</returns>
    public async Task<AttendanceRecordDto> GetAsync(Guid id)
    {
        var record = await _attendanceRepository.GetAsync(id);
        var dto = _mapper.AttendanceRecordToDto(record);
        await FillAttendanceNamesAsync(new List<AttendanceRecordDto> { dto });
        return dto;
    }

    /// <summary>
    /// 獲取出勤記錄列表
    /// </summary>
    /// <param name="input">查詢參數，包含員工ID、部門ID、日期範圍和狀態等過濾條件</param>
    /// <returns>分頁的出勤記錄列表</returns>
    public async Task<PagedResultDto<AttendanceRecordDto>> GetListAsync(GetAttendanceInput input)
    {
        var query = await _attendanceRepository.GetQueryableAsync();

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

        if (input.DateFrom.HasValue)
        {
            query = query.Where(x => x.Date >= input.DateFrom.Value.Date);
        }

        if (input.DateTo.HasValue)
        {
            query = query.Where(x => x.Date <= input.DateTo.Value.Date);
        }

        if (input.Status.HasValue)
        {
            query = query.Where(x => x.Status == input.Status);
        }

        var totalCount = query.Count();

        query = query.OrderByDescending(x => x.Date);

        var items = query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        var dtos = items.Select(_mapper.AttendanceRecordToDto).ToList();
        await FillAttendanceNamesAsync(dtos);

        return new PagedResultDto<AttendanceRecordDto>(totalCount, dtos);
    }

    /// <summary>
    /// 員工打卡上班
    /// </summary>
    /// <param name="input">打卡參數，包含員工ID和備註</param>
    /// <returns>打卡後的出勤記錄</returns>
    [Authorize(EipPermissions.HumanResources.Attendance.ClockInOut)]
    public async Task<AttendanceRecordDto> ClockInAsync(ClockInOutDto input)
    {
        var employee = await GetEmployeeAsync(input.EmployeeId);
        var today = Clock.Now.Date;

        var existingRecord = await _attendanceRepository.FirstOrDefaultAsync(
            x => x.EmployeeId == employee.Id && x.Date == today);

        if (existingRecord != null && existingRecord.ClockIn.HasValue)
        {
            throw new BusinessException("HumanResources:AlreadyClockedIn");
        }

        AttendanceRecord record;
        if (existingRecord != null)
        {
            record = existingRecord;
        }
        else
        {
            record = new AttendanceRecord(
                GuidGenerator.Create(),
                employee.Id,
                today,
                CurrentTenant.Id);
        }

        record.ClockInNow(Clock.Now, _workStartTime);
        record.Note = input.Note;

        if (existingRecord != null)
        {
            await _attendanceRepository.UpdateAsync(record);
        }
        else
        {
            await _attendanceRepository.InsertAsync(record);
        }

        var dto = _mapper.AttendanceRecordToDto(record);
        await FillAttendanceNamesAsync(new List<AttendanceRecordDto> { dto });
        return dto;
    }

    /// <summary>
    /// 員工打卡下班
    /// </summary>
    /// <param name="input">打卡參數，包含員工ID和備註</param>
    /// <returns>打卡後的出勤記錄</returns>
    [Authorize(EipPermissions.HumanResources.Attendance.ClockInOut)]
    public async Task<AttendanceRecordDto> ClockOutAsync(ClockInOutDto input)
    {
        var employee = await GetEmployeeAsync(input.EmployeeId);
        var today = Clock.Now.Date;

        var record = await _attendanceRepository.FirstOrDefaultAsync(
            x => x.EmployeeId == employee.Id && x.Date == today) ?? throw new BusinessException("HumanResources:NotClockedIn");

        if (record.ClockOut.HasValue)
        {
            throw new BusinessException("HumanResources:AlreadyClockedOut");
        }

        record.ClockOutNow(Clock.Now, _workEndTime);
        if (!string.IsNullOrWhiteSpace(input.Note))
        {
            record.Note = string.IsNullOrWhiteSpace(record.Note)
                ? input.Note
                : $"{record.Note}; {input.Note}";
        }

        await _attendanceRepository.UpdateAsync(record);

        var dto = _mapper.AttendanceRecordToDto(record);
        await FillAttendanceNamesAsync(new List<AttendanceRecordDto> { dto });
        return dto;
    }

    /// <summary>
    /// 更新出勤記錄
    /// </summary>
    /// <param name="id">出勤記錄ID</param>
    /// <param name="input">更新參數，包含打卡時間、狀態、工作時數等信息</param>
    /// <returns>更新後的出勤記錄</returns>
    [Authorize(EipPermissions.HumanResources.Attendance.ManageRecords)]
    public async Task<AttendanceRecordDto> UpdateAsync(Guid id, UpdateAttendanceRecordDto input)
    {
        var record = await _attendanceRepository.GetAsync(id);

        record.ClockIn = input.ClockIn;
        record.ClockOut = input.ClockOut;
        record.Status = input.Status;
        record.WorkHours = input.WorkHours;
        record.OvertimeHours = input.OvertimeHours;
        record.Note = input.Note;

        await _attendanceRepository.UpdateAsync(record);

        var dto = _mapper.AttendanceRecordToDto(record);
        await FillAttendanceNamesAsync(new List<AttendanceRecordDto> { dto });
        return dto;
    }

    /// <summary>
    /// 獲取今日出勤記錄
    /// </summary>
    /// <param name="employeeId">員工ID，如果未提供則使用當前用戶</param>
    /// <returns>今日出勤記錄，如果沒有則返回null</returns>
    public async Task<AttendanceRecordDto?> GetTodayRecordAsync(Guid? employeeId = null)
    {
        var employee = await GetEmployeeAsync(employeeId);
        var today = Clock.Now.Date;

        var record = await _attendanceRepository.FirstOrDefaultAsync(
            x => x.EmployeeId == employee.Id && x.Date == today);

        if (record == null)
        {
            return null;
        }

        var dto = _mapper.AttendanceRecordToDto(record);
        await FillAttendanceNamesAsync(new List<AttendanceRecordDto> { dto });
        return dto;
    }

    /// <summary>
    /// 獲取月度出勤報表
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="departmentId">部門ID，可選，如果提供則只返回該部門的報表</param>
    /// <returns>月度出勤報表列表</returns>
    [Authorize(EipPermissions.HumanResources.Attendance.ViewReports)]
    public async Task<List<AttendanceReportDto>> GetMonthlyReportAsync(int year, int month, Guid? departmentId = null)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var employeeQuery = await _employeeRepository.GetQueryableAsync();
        employeeQuery = employeeQuery.Where(x => x.Status == EmployeeStatus.Active);

        if (departmentId.HasValue)
        {
            employeeQuery = employeeQuery.Where(x => x.DepartmentId == departmentId);
        }

        var employees = employeeQuery.ToList();
        var employeeIds = employees.Select(x => x.Id).ToList();

        var attendances = (await _attendanceRepository.GetQueryableAsync())
            .Where(x => employeeIds.Contains(x.EmployeeId) && x.Date >= startDate && x.Date <= endDate)
            .ToList();

        var reports = new List<AttendanceReportDto>();
        var workDays = CountWorkDays(startDate, endDate);

        foreach (var employee in employees)
        {
            var empAttendances = attendances.Where(x => x.EmployeeId == employee.Id).ToList();

            reports.Add(new AttendanceReportDto
            {
                EmployeeId = employee.Id,
                EmployeeName = employee.Name,
                EmployeeNo = employee.EmployeeNo,
                Year = year,
                Month = month,
                WorkDays = workDays,
                ActualWorkDays = empAttendances.Count(x => x.Status == AttendanceStatus.Normal),
                LateDays = empAttendances.Count(x => x.Status == AttendanceStatus.Late),
                EarlyLeaveDays = empAttendances.Count(x => x.Status == AttendanceStatus.EarlyLeave),
                AbsentDays = empAttendances.Count(x => x.Status == AttendanceStatus.Absent),
                LeaveDays = empAttendances.Count(x => x.Status == AttendanceStatus.OnLeave),
                TotalWorkHours = empAttendances.Sum(x => x.WorkHours),
                TotalOvertimeHours = empAttendances.Sum(x => x.OvertimeHours)
            });
        }

        return reports.OrderBy(x => x.EmployeeNo).ToList();
    }

    /// <summary>
    /// 獲取員工信息
    /// </summary>
    /// <param name="employeeId">員工ID，如果未提供則使用當前用戶</param>
    /// <returns>員工信息</returns>
    private async Task<Employee> GetEmployeeAsync(Guid? employeeId)
    {
        if (employeeId.HasValue)
        {
            return await _employeeRepository.GetAsync(employeeId.Value);
        }

        if (CurrentUser.Id == null)
        {
            throw new BusinessException("HumanResources:UserNotLoggedIn");
        }

        var employee = await _employeeRepository.FirstOrDefaultAsync(x => x.UserId == CurrentUser.Id) ?? throw new BusinessException("HumanResources:EmployeeNotFound");

        return employee;
    }

    /// <summary>
    /// 計算工作天數（週一至週五）
    /// </summary>
    /// <param name="start">開始日期</param>
    /// <param name="end">結束日期</param>
    /// <returns>工作天數</returns>
    private int CountWorkDays(DateTime start, DateTime end)
    {
        var count = 0;
        for (var date = start; date <= end; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// 填充出勤記錄相關的員工姓名信息
    /// </summary>
    /// <param name="dtos">出勤記錄DTO列表</param>
    private async Task FillAttendanceNamesAsync(List<AttendanceRecordDto> dtos)
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
