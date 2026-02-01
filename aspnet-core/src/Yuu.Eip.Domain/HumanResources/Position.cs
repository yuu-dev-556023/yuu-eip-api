using System;
using System.ComponentModel;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Yuu.Eip.HumanResources;

/// <summary>
/// 職位
/// </summary>
[Description("職位")]
public class Position : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 租戶 Id
    /// </summary>
    [Description("租戶Id")]
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 職位代碼
    /// </summary>
    [Description("職位代碼")]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 職位名稱
    /// </summary>
    [Description("職位名稱")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 職等
    /// </summary>
    [Description("職等")]
    public int Level { get; set; }

    /// <summary>
    /// 底薪
    /// </summary>
    [Description("底薪")]
    public decimal BaseSalary { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    [Description("是否啟用")]
    public bool IsActive { get; set; } = true;

    protected Position()
    {
    }

    public Position(
        Guid id,
        string code,
        string name,
        int level,
        decimal baseSalary,
        bool isActive = true,
        Guid? tenantId = null)
        : base(id)
    {
        Code = code;
        Name = name;
        Level = level;
        BaseSalary = baseSalary;
        IsActive = isActive;
        TenantId = tenantId;
    }
}
