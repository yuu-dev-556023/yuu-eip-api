using System;
using System.ComponentModel;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Yuu.Eip.HumanResources;

/// <summary>
/// 請假單
/// </summary>
[Description("請假單")]
public class LeaveRequest : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 租戶 Id
    /// </summary>
    [Description("租戶Id")]
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// 假別
    /// </summary>
    [Description("假別")]
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
    /// 請假事由
    /// </summary>
    [Description("請假事由")]
    public string? Reason { get; set; }

    /// <summary>
    /// 請假單狀態
    /// </summary>
    [Description("請假單狀態")]
    public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

    /// <summary>
    /// 審核人 Id
    /// </summary>
    [Description("審核人Id")]
    public Guid? ApproverId { get; set; }

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

    protected LeaveRequest()
    {
    }

    public LeaveRequest(
        Guid id,
        Guid employeeId,
        LeaveType leaveType,
        DateTime startDate,
        DateTime endDate,
        decimal hours,
        string? reason = null,
        Guid? tenantId = null)
        : base(id)
    {
        EmployeeId = employeeId;
        LeaveType = leaveType;
        StartDate = startDate;
        EndDate = endDate;
        Hours = hours;
        Reason = reason;
        Status = LeaveRequestStatus.Pending;
        TenantId = tenantId;
    }

    /// <summary>
    /// 核准
    /// </summary>
    public void Approve(Guid approverId, string? comment = null)
    {
        Status = LeaveRequestStatus.Approved;
        ApproverId = approverId;
        ApprovedTime = DateTime.UtcNow;
        ApproverComment = comment;
    }

    /// <summary>
    /// 駁回
    /// </summary>
    public void Reject(Guid approverId, string? comment = null)
    {
        Status = LeaveRequestStatus.Rejected;
        ApproverId = approverId;
        ApprovedTime = DateTime.UtcNow;
        ApproverComment = comment;
    }

    /// <summary>
    /// 取消
    /// </summary>
    public void Cancel()
    {
        Status = LeaveRequestStatus.Cancelled;
    }
}
