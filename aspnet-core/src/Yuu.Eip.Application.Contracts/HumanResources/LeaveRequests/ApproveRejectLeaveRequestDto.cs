using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yuu.Eip.HumanResources.LeaveRequests;

/// <summary>
/// 審核請假申請 DTO
/// </summary>
[Description("審核請假申請")]
public class ApproveRejectLeaveRequestDto
{
    /// <summary>
    /// 審核意見
    /// </summary>
    [Description("審核意見")]
    [StringLength(HumanResourcesConsts.MaxApproverCommentLength)]
    public string? Comment { get; set; }
}
