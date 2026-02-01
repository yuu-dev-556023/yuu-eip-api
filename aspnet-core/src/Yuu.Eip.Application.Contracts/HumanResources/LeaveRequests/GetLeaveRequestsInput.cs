using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.LeaveRequests;

/// <summary>
/// 查詢請假申請輸入參數
/// </summary>
[Description("查詢請假申請")]
public class GetLeaveRequestsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    public Guid? EmployeeId { get; set; }

    /// <summary>
    /// 請假類型
    /// </summary>
    [Description("請假類型")]
    public LeaveType? LeaveType { get; set; }

    /// <summary>
    /// 申請狀態
    /// </summary>
    [Description("申請狀態")]
    public LeaveRequestStatus? Status { get; set; }

    /// <summary>
    /// 開始日期起
    /// </summary>
    [Description("開始日期起")]
    public DateTime? StartDateFrom { get; set; }

    /// <summary>
    /// 開始日期止
    /// </summary>
    [Description("開始日期止")]
    public DateTime? StartDateTo { get; set; }
}
