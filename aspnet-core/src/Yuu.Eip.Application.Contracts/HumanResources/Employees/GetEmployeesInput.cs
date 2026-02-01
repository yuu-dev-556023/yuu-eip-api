using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Employees;

/// <summary>
/// 查詢員工清單輸入參數
/// </summary>
[Description("查詢員工清單")]
public class GetEmployeesInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 篩選條件
    /// </summary>
    [Description("篩選條件")]
    public string? Filter { get; set; }

    /// <summary>
    /// 部門 Id
    /// </summary>
    [Description("部門Id")]
    public Guid? DepartmentId { get; set; }

    /// <summary>
    /// 職位 Id
    /// </summary>
    [Description("職位Id")]
    public Guid? PositionId { get; set; }

    /// <summary>
    /// 員工狀態
    /// </summary>
    [Description("員工狀態")]
    public EmployeeStatus? Status { get; set; }
}
