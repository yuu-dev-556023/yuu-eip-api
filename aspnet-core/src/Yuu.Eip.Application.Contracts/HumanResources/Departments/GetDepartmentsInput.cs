using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Departments;

/// <summary>
/// 查詢部門清單輸入參數
/// </summary>
[Description("查詢部門清單")]
public class GetDepartmentsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 篩選條件
    /// </summary>
    [Description("篩選條件")]
    public string? Filter { get; set; }

    /// <summary>
    /// 上級部門 Id
    /// </summary>
    [Description("上級部門Id")]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Description("是否啟用")]
    public bool? IsActive { get; set; }
}
