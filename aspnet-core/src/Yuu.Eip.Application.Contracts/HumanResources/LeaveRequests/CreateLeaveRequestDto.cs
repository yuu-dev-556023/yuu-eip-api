using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yuu.Eip.HumanResources.LeaveRequests;

/// <summary>
/// 創建請假申請 DTO
/// </summary>
[Description("創建請假申請")]
public class CreateLeaveRequestDto
{
    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    [Required]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// 請假類型
    /// </summary>
    [Description("請假類型")]
    [Required]
    public LeaveType LeaveType { get; set; }

    /// <summary>
    /// 開始日期
    /// </summary>
    [Description("開始日期")]
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 結束日期
    /// </summary>
    [Description("結束日期")]
    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// 請假時數
    /// </summary>
    [Description("請假時數")]
    [Required]
    [Range(0.5, 240)]
    public decimal Hours { get; set; }

    /// <summary>
    /// 請假原因
    /// </summary>
    [Description("請假原因")]
    [StringLength(HumanResourcesConsts.MaxLeaveReasonLength)]
    public string? Reason { get; set; }
}
