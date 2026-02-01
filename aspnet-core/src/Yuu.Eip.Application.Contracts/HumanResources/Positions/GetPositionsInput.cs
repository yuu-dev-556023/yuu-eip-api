using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Positions;

/// <summary>
/// 查詢職位輸入參數
/// </summary>
[Description("查詢職位")]
public class GetPositionsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 篩選條件
    /// </summary>
    [Description("篩選條件")]
    public string? Filter { get; set; }

    /// <summary>
    /// 職位等級
    /// </summary>
    [Description("職位等級")]
    public int? Level { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Description("是否啟用")]
    public bool? IsActive { get; set; }
}
