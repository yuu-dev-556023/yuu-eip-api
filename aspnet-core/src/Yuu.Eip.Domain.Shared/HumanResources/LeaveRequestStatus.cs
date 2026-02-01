namespace Yuu.Eip.HumanResources;

/// <summary>
/// 請假單狀態
/// </summary>
public enum LeaveRequestStatus
{
    /// <summary>
    /// 待審核
    /// </summary>
    Pending = 1,

    /// <summary>
    /// 已核准
    /// </summary>
    Approved = 2,

    /// <summary>
    /// 已駁回
    /// </summary>
    Rejected = 3,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled = 4
}
