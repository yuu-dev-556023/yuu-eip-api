using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Attendance;

/// <summary>
/// 查詢考勤記錄輸入參數
/// </summary>
[Description("查詢考勤記錄")]
public class GetAttendanceInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    public Guid? EmployeeId { get; set; }

    /// <summary>
    /// 部門 Id
    /// </summary>
    [Description("部門Id")]
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// 開始日期
    /// </summary>
    [Description("開始日期")]
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// 結束日期
    /// </summary>
    [Description("結束日期")]
    public DateTime? DateTo { get; set; }

    /// <summary>
    /// 考勤狀態
    /// </summary>
    [Description("考勤狀態")]
    public AttendanceStatus? Status { get; set; }
}
