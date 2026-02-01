using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;
using Yuu.Eip.Controllers;
using Yuu.Eip.HumanResources.Attendance;

namespace Yuu.Eip.HumanResources;

[SwaggerTag("出勤管理")]
public class AttendanceController : EipController
{
    private readonly IAttendanceAppService _attendanceAppService;

    /// <summary>
    /// 初始化出勤控制器
    /// </summary>
    /// <param name="attendanceAppService">出勤應用服務</param>
    public AttendanceController(IAttendanceAppService attendanceAppService)
    {
        _attendanceAppService = attendanceAppService;
    }

    /// <summary>
    /// 獲取出勤記錄列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的出勤記錄列表</returns>
    [HttpGet]
    public Task<PagedResultDto<AttendanceRecordDto>> GetListAsync([FromQuery] GetAttendanceInput input)
    {
        return _attendanceAppService.GetListAsync(input);
    }

    /// <summary>
    /// 獲取指定ID的出勤記錄
    /// </summary>
    /// <param name="id">出勤記錄ID</param>
    /// <returns>出勤記錄詳情</returns>
    [HttpGet("{id}")]
    public Task<AttendanceRecordDto> GetAsync(Guid id)
    {
        return _attendanceAppService.GetAsync(id);
    }

    /// <summary>
    /// 員工打卡上班
    /// </summary>
    /// <param name="input">打卡參數</param>
    /// <returns>打卡後的出勤記錄</returns>
    [HttpPost("clock-in")]
    public Task<AttendanceRecordDto> ClockInAsync([FromBody] ClockInOutDto input)
    {
        return _attendanceAppService.ClockInAsync(input);
    }

    /// <summary>
    /// 員工打卡下班
    /// </summary>
    /// <param name="input">打卡參數</param>
    /// <returns>打卡後的出勤記錄</returns>
    [HttpPost("clock-out")]
    public Task<AttendanceRecordDto> ClockOutAsync([FromBody] ClockInOutDto input)
    {
        return _attendanceAppService.ClockOutAsync(input);
    }

    /// <summary>
    /// 更新出勤記錄
    /// </summary>
    /// <param name="id">出勤記錄ID</param>
    /// <param name="input">更新參數</param>
    /// <returns>更新後的出勤記錄</returns>
    [HttpPut("{id}")]
    public Task<AttendanceRecordDto> UpdateAsync(Guid id, [FromBody] UpdateAttendanceRecordDto input)
    {
        return _attendanceAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 獲取今日出勤記錄
    /// </summary>
    /// <param name="employeeId">員工ID，可選</param>
    /// <returns>今日出勤記錄</returns>
    [HttpGet("today")]
    public Task<AttendanceRecordDto?> GetTodayRecordAsync([FromQuery] Guid? employeeId = null)
    {
        return _attendanceAppService.GetTodayRecordAsync(employeeId);
    }

    /// <summary>
    /// 獲取月度出勤報表
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="departmentId">部門ID，可選</param>
    /// <returns>月度出勤報表列表</returns>
    [HttpGet("monthly-report")]
    public Task<List<AttendanceReportDto>> GetMonthlyReportAsync([FromQuery] int year, [FromQuery] int month, [FromQuery] Guid? departmentId = null)
    {
        return _attendanceAppService.GetMonthlyReportAsync(year, month, departmentId);
    }
}
