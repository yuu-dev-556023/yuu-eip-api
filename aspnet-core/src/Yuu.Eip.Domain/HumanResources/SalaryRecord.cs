using System;
using System.ComponentModel;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Yuu.Eip.HumanResources;

/// <summary>
/// 薪資記錄
/// </summary>
[Description("薪資記錄")]
public class SalaryRecord : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 租戶 Id
    /// </summary>
    [Description("租戶Id")]
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// 年
    /// </summary>
    [Description("年")]
    public int Year { get; set; }

    /// <summary>
    /// 月
    /// </summary>
    [Description("月")]
    public int Month { get; set; }

    /// <summary>
    /// 底薪
    /// </summary>
    [Description("底薪")]
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
    /// 扣款 (請假/遲到等)
    /// </summary>
    [Description("扣款")]
    public decimal Deductions { get; set; }

    /// <summary>
    /// 勞保
    /// </summary>
    [Description("勞保")]
    public decimal LaborInsurance { get; set; }

    /// <summary>
    /// 健保
    /// </summary>
    [Description("健保")]
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
    public SalaryStatus Status { get; set; } = SalaryStatus.Pending;

    /// <summary>
    /// 備註
    /// </summary>
    [Description("備註")]
    public string? Note { get; set; }

    protected SalaryRecord()
    {
    }

    public SalaryRecord(
        Guid id,
        Guid employeeId,
        int year,
        int month,
        Guid? tenantId = null)
        : base(id)
    {
        EmployeeId = employeeId;
        Year = year;
        Month = month;
        Status = SalaryStatus.Pending;
        TenantId = tenantId;
    }

    /// <summary>
    /// 計算薪資
    /// </summary>
    public void Calculate(
        decimal baseSalary,
        decimal overtimePay,
        decimal bonus,
        decimal deductions,
        decimal laborInsuranceRate = HumanResourcesConsts.LaborInsuranceRate,
        decimal healthInsuranceRate = HumanResourcesConsts.HealthInsuranceRate)
    {
        BaseSalary = baseSalary;
        OvertimePay = overtimePay;
        Bonus = bonus;
        Deductions = deductions;
        LaborInsurance = Math.Round(baseSalary * laborInsuranceRate, 0);
        HealthInsurance = Math.Round(baseSalary * healthInsuranceRate, 0);

        /* 實發薪資 = 底薪 + 加班費 + 獎金 - 扣款 - 勞保 - 健保 */
        NetSalary = BaseSalary + OvertimePay + Bonus - Deductions - LaborInsurance - HealthInsurance;
    }

    /// <summary>
    /// 確認薪資
    /// </summary>
    public void Confirm()
    {
        Status = SalaryStatus.Confirmed;
    }

    /// <summary>
    /// 標記為已發放
    /// </summary>
    public void MarkAsPaid()
    {
        Status = SalaryStatus.Paid;
    }
}
