using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Yuu.Eip.HumanResources.Attendance;

/// <summary>
/// 出勤管理應用服務接口
/// </summary>
public interface IAttendanceAppService : IApplicationService
{
    /// <summary>
    /// 獲取指定ID的出勤記錄
    /// </summary>
    /// <param name="id">出勤記錄ID</param>
    /// <returns>出勤記錄詳情</returns>
    Task<AttendanceRecordDto> GetAsync(Guid id);

    /// <summary>
    /// 獲取出勤記錄列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的出勤記錄列表</returns>
    Task<PagedResultDto<AttendanceRecordDto>> GetListAsync(GetAttendanceInput input);

    /// <summary>
    /// 員工打卡上班
    /// </summary>
    /// <param name="input">打卡參數</param>
    /// <returns>打卡後的出勤記錄</returns>
    Task<AttendanceRecordDto> ClockInAsync(ClockInOutDto input);

    /// <summary>
    /// 員工打卡下班
    /// </summary>
    /// <param name="input">打卡參數</param>
    /// <returns>打卡後的出勤記錄</returns>
    Task<AttendanceRecordDto> ClockOutAsync(ClockInOutDto input);

    /// <summary>
    /// 更新出勤記錄
    /// </summary>
    /// <param name="id">出勤記錄ID</param>
    /// <param name="input">更新參數</param>
    /// <returns>更新後的出勤記錄</returns>
    Task<AttendanceRecordDto> UpdateAsync(Guid id, UpdateAttendanceRecordDto input);

    /// <summary>
    /// 獲取今日出勤記錄
    /// </summary>
    /// <param name="employeeId">員工ID，可選</param>
    /// <returns>今日出勤記錄</returns>
    Task<AttendanceRecordDto?> GetTodayRecordAsync(Guid? employeeId = null);

    /// <summary>
    /// 獲取月度出勤報表
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="departmentId">部門ID，可選</param>
    /// <returns>月度出勤報表列表</returns>
    Task<List<AttendanceReportDto>> GetMonthlyReportAsync(int year, int month, Guid? departmentId = null);
}
