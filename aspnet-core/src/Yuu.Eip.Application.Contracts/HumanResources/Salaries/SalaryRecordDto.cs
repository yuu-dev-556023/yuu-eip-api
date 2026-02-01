using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Salaries;

/// <summary>
/// 薪資記錄 DTO
/// </summary>
[Description("薪資記錄")]
public class SalaryRecordDto : FullAuditedEntityDto<Guid>
{
    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// 員工姓名
    /// </summary>
    [Description("員工姓名")]
    public string? EmployeeName { get; set; }

    /// <summary>
    /// 員工編號
    /// </summary>
    [Description("員工編號")]
    public string? EmployeeNo { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [Description("年份")]
    public int Year { get; set; }

    /// <summary>
    /// 月份
    /// </summary>
    [Description("月份")]
    public int Month { get; set; }

    /// <summary>
    /// 基本薪資
    /// </summary>
    [Description("基本薪資")]
    public decimal BaseSalary { get; set; }

    /// <summary>
    /// 加班費
    /// </summary>
    [Description("加班費")]
    public decimal OvertimePay { get; set; }

    /// <summary>
    /// 獎金
    /// </summary>
    [Description("獎金")]
    public decimal Bonus { get; set; }

    /// <summary>
    /// 其他扣款
    /// </summary>
    [Description("其他扣款")]
    public decimal Deductions { get; set; }

    /// <summary>
    /// 勞保費用
    /// </summary>
    [Description("勞保費用")]
    public decimal LaborInsurance { get; set; }

    /// <summary>
    /// 健保費用
    /// </summary>
    [Description("健保費用")]
    public decimal HealthInsurance { get; set; }

    /// <summary>
    /// 實發薪資
    /// </summary>
    [Description("實發薪資")]
    public decimal NetSalary { get; set; }

    /// <summary>
    /// 薪資狀態
    /// </summary>
    [Description("薪資狀態")]
    public SalaryStatus Status { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Description("備註")]
    public string? Note { get; set; }
}
