using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.LeaveRequests;

/// <summary>
/// 請假申請 DTO
/// </summary>
[Description("請假申請")]
public class LeaveRequestDto : FullAuditedEntityDto<Guid>
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
    /// 請假類型
    /// </summary>
    [Description("請假類型")]
    public LeaveType LeaveType { get; set; }

    /// <summary>
    /// 開始日期
    /// </summary>
    [Description("開始日期")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 結束日期
    /// </summary>
    [Description("結束日期")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 請假時數
    /// </summary>
    [Description("請假時數")]
    public decimal Hours { get; set; }

    /// <summary>
    /// 請假原因
    /// </summary>
    [Description("請假原因")]
    public string? Reason { get; set; }

    /// <summary>
    /// 申請狀態
    /// </summary>
    [Description("申請狀態")]
    public LeaveRequestStatus Status { get; set; }

    /// <summary>
    /// 審核者 Id
    /// </summary>
    [Description("審核者Id")]
    public Guid? ApproverId { get; set; }

    /// <summary>
    /// 審核者姓名
    /// </summary>
    [Description("審核者姓名")]
    public string? ApproverName { get; set; }

    /// <summary>
    /// 審核時間
    /// </summary>
    [Description("審核時間")]
    public DateTime? ApprovedTime { get; set; }

    /// <summary>
    /// 審核意見
    /// </summary>
    [Description("審核意見")]
    public string? ApproverComment { get; set; }
}
