using System;
using System.ComponentModel;

namespace Yuu.Eip.HumanResources.Salaries;

/// <summary>
/// 薪資單 DTO
/// </summary>
[Description("薪資單")]
public class SalarySlipDto
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
    /// 部門名稱
    /// </summary>
    [Description("部門名稱")]
    public string? DepartmentName { get; set; }

    /// <summary>
    /// 職位名稱
    /// </summary>
    [Description("職位名稱")]
    public string? PositionName { get; set; }

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

    /* 收入 */
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
    /// 總收入
    /// </summary>
    [Description("總收入")]
    public decimal GrossIncome => BaseSalary + OvertimePay + Bonus;

    /* 扣款 */
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
    /// 總扣款
    /// </summary>
    [Description("總扣款")]
    public decimal TotalDeductions => Deductions + LaborInsurance + HealthInsurance;

    /* 實發 */
    /// <summary>
    /// 實發薪資
    /// </summary>
    [Description("實發薪資")]
    public decimal NetSalary { get; set; }

    /* 出勤資訊 */
    /// <summary>
    /// 工作天數
    /// </summary>
    [Description("工作天數")]
    public int WorkDays { get; set; }

    /// <summary>
    /// 總工作時數
    /// </summary>
    [Description("總工作時數")]
    public decimal TotalWorkHours { get; set; }

    /// <summary>
    /// 總加班時數
    /// </summary>
    [Description("總加班時數")]
    public decimal TotalOvertimeHours { get; set; }

    /// <summary>
    /// 遲到天數
    /// </summary>
    [Description("遲到天數")]
    public int LateDays { get; set; }

    /// <summary>
    /// 請假天數
    /// </summary>
    [Description("請假天數")]
    public int LeaveDays { get; set; }
}
