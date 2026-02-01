using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yuu.Eip.HumanResources.Attendance;

/// <summary>
/// 更新考勤記錄 DTO
/// </summary>
[Description("更新考勤記錄")]
public class UpdateAttendanceRecordDto
{
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
    [StringLength(HumanResourcesConsts.MaxNoteLength)]
    public string? Note { get; set; }
}
