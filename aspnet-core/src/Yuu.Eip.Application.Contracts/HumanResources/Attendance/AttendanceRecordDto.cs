using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Attendance;

/// <summary>
/// 考勤記錄 DTO
/// </summary>
[Description("考勤記錄")]
public class AttendanceRecordDto : FullAuditedEntityDto<Guid>
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
    /// 考勤日期
    /// </summary>
    [Description("考勤日期")]
    public DateTime Date { get; set; }

    /// <summary>
    /// 簽到時間
    /// </summary>
    [Description("簽到時間")]
    public DateTime? ClockIn { get; set; }

    /// <summary>
    /// 簽退時間
    /// </summary>
    [Description("簽退時間")]
    public DateTime? ClockOut { get; set; }

    /// <summary>
    /// 考勤狀態
    /// </summary>
    [Description("考勤狀態")]
    public AttendanceStatus Status { get; set; }

    /// <summary>
    /// 工作時數
    /// </summary>
    [Description("工作時數")]
    public decimal WorkHours { get; set; }

    /// <summary>
    /// 加班時數
    /// </summary>
    [Description("加班時數")]
    public decimal OvertimeHours { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Description("備註")]
    public string? Note { get; set; }
}
