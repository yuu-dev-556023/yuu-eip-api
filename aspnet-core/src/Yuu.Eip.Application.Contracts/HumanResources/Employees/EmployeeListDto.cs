using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Employees;

/// <summary>
/// 員工列表 DTO
/// </summary>
[Description("員工列表")]
public class EmployeeListDto : EntityDto<Guid>
{
    /// <summary>
    /// 員工編號
    /// </summary>
    [Description("員工編號")]
    public string EmployeeNo { get; set; } = null!;

    /// <summary>
    /// 姓名
    /// </summary>
    [Description("姓名")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 電子郵件
    /// </summary>
    [Description("電子郵件")]
    public string? Email { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    [Description("電話")]
    public string? Phone { get; set; }

    /// <summary>
    /// 員工狀態
    /// </summary>
    [Description("員工狀態")]
    public EmployeeStatus Status { get; set; }

    /// <summary>
    /// 所屬部門名稱
    /// </summary>
    [Description("所屬部門名稱")]
    public string? DepartmentName { get; set; }

    /// <summary>
    /// 職位名稱
    /// </summary>
    [Description("職位名稱")]
    public string? PositionName { get; set; }

    /// <summary>
    /// 到職日
    /// </summary>
    [Description("到職日")]
    public DateTime HireDate { get; set; }
}
