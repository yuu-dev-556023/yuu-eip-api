using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Salaries;

/// <summary>
/// 查詢薪資記錄輸入參數
/// </summary>
[Description("查詢薪資記錄")]
public class GetSalaryRecordsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    public Guid? EmployeeId { get; set; }

    /// <summary>
    /// 部門 Id
    /// </summary>
    [Description("部門Id")]
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// 年份
    /// </summary>
    [Description("年份")]
    public int? Year { get; set; }

    /// <summary>
    /// 月份
    /// </summary>
    [Description("月份")]
    public int? Month { get; set; }

    /// <summary>
    /// 薪資狀態
    /// </summary>
    [Description("薪資狀態")]
    public SalaryStatus? Status { get; set; }
}
