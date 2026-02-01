namespace Yuu.Eip.HumanResources;

/// <summary>
/// 出勤狀態
/// </summary>
public enum AttendanceStatus
{
    /// <summary>
    /// 正常
    /// </summary>
    Normal = 1,

    /// <summary>
    /// 遲到
    /// </summary>
    Late = 2,

    /// <summary>
    /// 早退
    /// </summary>
    EarlyLeave = 3,

    /// <summary>
    /// 曠職
    /// </summary>
    Absent = 4,

    /// <summary>
    /// 請假
    /// </summary>
    OnLeave = 5
}
