using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yuu.Eip.HumanResources.Positions;

/// <summary>
/// 創建更新職位 DTO
/// </summary>
[Description("創建更新職位")]
public class CreateUpdatePositionDto
{
    /// <summary>
    /// 職位代碼
    /// </summary>
    [Description("職位代碼")]
    [Required]
    [StringLength(HumanResourcesConsts.MaxPositionCodeLength)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 職位名稱
    /// </summary>
    [Description("職位名稱")]
    [Required]
    [StringLength(HumanResourcesConsts.MaxPositionNameLength)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 職位等級
    /// </summary>
    [Description("職位等級")]
    [Range(1, 100)]
    public int Level { get; set; }

    /// <summary>
    /// 基本薪資
    /// </summary>
    [Description("基本薪資")]
    [Range(0, double.MaxValue)]
    public decimal BaseSalary { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Description("是否啟用")]
    public bool IsActive { get; set; } = true;
}
