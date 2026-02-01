using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Departments;

/// <summary>
/// 部門 DTO
/// </summary>
[Description("部門")]
public class DepartmentDto : FullAuditedEntityDto<Guid>
{
    /// <summary>
    /// 部門代碼
    /// </summary>
    [Description("部門代碼")]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 部門名稱
    /// </summary>
    [Description("部門名稱")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 上級部門 Id
    /// </summary>
    [Description("上級部門Id")]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 上級部門名稱
    /// </summary>
    [Description("上級部門名稱")]
    public string? ParentName { get; set; }

    /// <summary>
    /// 部門主管 Id
    /// </summary>
    [Description("部門主管Id")]
    public Guid? ManagerId { get; set; }

    /// <summary>
    /// 部門主管名稱
    /// </summary>
    [Description("部門主管名稱")]
    public string? ManagerName { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Description("排序")]
    public int Order { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Description("是否啟用")]
    public bool IsActive { get; set; }
}
