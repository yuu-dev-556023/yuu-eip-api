using System;
using System.ComponentModel;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Yuu.Eip.HumanResources;

/// <summary>
/// 部門
/// </summary>
[Description("部門")]
public class Department : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 租戶 Id
    /// </summary>
    [Description("租戶Id")]
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 部門代碼 (唯一)
    /// </summary>
    [Description("部門代碼")]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 部門名稱
    /// </summary>
    [Description("部門名稱")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 上級部門 Id (樹狀結構)
    /// </summary>
    [Description("上級部門Id")]
    public Guid? ParentId { get; set; }

    /// <summary>
    /// 部門主管 Id
    /// </summary>
    [Description("部門主管Id")]
    public Guid? ManagerId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Description("排序")]
    public int Order { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Description("是否啟用")]
    public bool IsActive { get; set; } = true;

    protected Department()
    {
    }

    public Department(
        Guid id,
        string code,
        string name,
        Guid? parentId = null,
        Guid? managerId = null,
        int order = 0,
        bool isActive = true,
        Guid? tenantId = null)
        : base(id)
    {
        Code = code;
        Name = name;
        ParentId = parentId;
        ManagerId = managerId;
        Order = order;
        IsActive = isActive;
        TenantId = tenantId;
    }
}
