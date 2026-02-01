using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Positions;

/// <summary>
/// 職位 DTO
/// </summary>
[Description("職位")]
public class PositionDto : FullAuditedEntityDto<Guid>
{
    /// <summary>
    /// 職位代碼
    /// </summary>
    [Description("職位代碼")]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 職位名稱
    /// </summary>
    [Description("職位名稱")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 職位等級
    /// </summary>
    [Description("職位等級")]
    public int Level { get; set; }

    /// <summary>
    /// 基本薪資
    /// </summary>
    [Description("基本薪資")]
    public decimal BaseSalary { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Description("是否啟用")]
    public bool IsActive { get; set; }
}
