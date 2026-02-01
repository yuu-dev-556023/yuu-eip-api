using System;
using System.ComponentModel;

namespace Yuu.Eip.HumanResources.Attendance;

/// <summary>
/// 考勤報表 DTO
/// </summary>
[Description("考勤報表")]
public class AttendanceReportDto
{
    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// 員工姓名
    /// </summary>
    [Description("員工姓名")]
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 員工編號
    /// </summary>
    [Description("員工編號")]
    public string? EmployeeNo { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [Description("年份")]
    public int Year { get; set; }

    /// <summary>
    /// 月份
    /// </summary>
    [Description("月份")]
    public int Month { get; set; }

    /// <summary>
    /// 工作天數
    /// </summary>
    [Description("工作天數")]
    public int WorkDays { get; set; }

    /// <summary>
    /// 實際工作天數
    /// </summary>
    [Description("實際工作天數")]
    public int ActualWorkDays { get; set; }

    /// <summary>
    /// 遲到天數
    /// </summary>
    [Description("遲到天數")]
    public int LateDays { get; set; }

    /// <summary>
    /// 早退天數
    /// </summary>
    [Description("早退天數")]
    public int EarlyLeaveDays { get; set; }

    /// <summary>
    /// 曠工天數
    /// </summary>
    [Description("曠工天數")]
    public int AbsentDays { get; set; }

    /// <summary>
    /// 請假天數
    /// </summary>
    [Description("請假天數")]
    public int LeaveDays { get; set; }

    /// <summary>
    /// 總工作時數
    /// </summary>
    [Description("總工作時數")]
    public decimal TotalWorkHours { get; set; }

    /// <summary>
    /// 總加班時數
    /// </summary>
    [Description("總加班時數")]
    public decimal TotalOvertimeHours { get; set; }
}
