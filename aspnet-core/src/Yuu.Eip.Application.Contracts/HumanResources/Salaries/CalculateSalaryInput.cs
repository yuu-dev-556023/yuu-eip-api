using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yuu.Eip.HumanResources.Salaries;

/// <summary>
/// 計算薪資輸入參數
/// </summary>
[Description("計算薪資")]
public class CalculateSalaryInput
{
    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    [Required]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [Description("年份")]
    [Required]
    [Range(2000, 2100)]
    public int Year { get; set; }

    /// <summary>
    /// 月份
    /// </summary>
    [Description("月份")]
    [Required]
    [Range(1, 12)]
    public int Month { get; set; }

    /// <summary>
    /// 獎金
    /// </summary>
    [Description("獎金")]
    public decimal Bonus { get; set; }
}
