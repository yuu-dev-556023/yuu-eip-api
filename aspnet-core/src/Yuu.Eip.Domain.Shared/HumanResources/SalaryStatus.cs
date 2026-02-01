namespace Yuu.Eip.HumanResources;

/// <summary>
/// 薪資狀態
/// </summary>
public enum SalaryStatus
{
    /// <summary>
    /// 待核算
    /// </summary>
    Pending = 1,

    /// <summary>
    /// 已確認
    /// </summary>
    Confirmed = 2,

    /// <summary>
    /// 已發放
    /// </summary>
    Paid = 3
}
