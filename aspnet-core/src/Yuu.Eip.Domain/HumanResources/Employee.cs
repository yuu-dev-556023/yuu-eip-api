using System;
using System.ComponentModel;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Yuu.Eip.HumanResources;

/// <summary>
/// 員工
/// </summary>
[Description("員工")]
public class Employee : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 租戶 Id
    /// </summary>
    [Description("租戶Id")]
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 員工編號 (唯一)
    /// </summary>
    [Description("員工編號")]
    public string EmployeeNo { get; set; } = null!;

    /// <summary>
    /// 姓名
    /// </summary>
    [Description("姓名")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Email
    /// </summary>
    [Description("電子郵件")]
    public string? Email { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    [Description("電話")]
    public string? Phone { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    [Description("生日")]
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// 性別
    /// </summary>
    [Description("性別")]
    public Gender Gender { get; set; }

    /// <summary>
    /// 到職日
    /// </summary>
    [Description("到職日")]
    public DateTime HireDate { get; set; }

    /// <summary>
    /// 離職日
    /// </summary>
    [Description("離職日")]
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 員工狀態
    /// </summary>
    [Description("員工狀態")]
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

    /// <summary>
    /// 所屬部門 Id
    /// </summary>
    [Description("所屬部門Id")]
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// 職位 Id
    /// </summary>
    [Description("職位Id")]
    public Guid PositionId { get; set; }

    /// <summary>
    /// 直屬主管 Id (自關聯)
    /// </summary>
    [Description("直屬主管Id")]
    public Guid? ManagerId { get; set; }

    /// <summary>
    /// 關聯 IdentityUser Id (可選)
    /// </summary>
    [Description("使用者Id")]
    public Guid? UserId { get; set; }

    protected Employee()
    {
    }

    public Employee(
        Guid id,
        string employeeNo,
        string name,
        Gender gender,
        DateTime hireDate,
        Guid departmentId,
        Guid positionId,
        string? email = null,
        string? phone = null,
        DateTime? birthDate = null,
        Guid? managerId = null,
        Guid? userId = null,
        Guid? tenantId = null)
        : base(id)
    {
        EmployeeNo = employeeNo;
        Name = name;
        Gender = gender;
        HireDate = hireDate;
        DepartmentId = departmentId;
        PositionId = positionId;
        Email = email;
        Phone = phone;
        BirthDate = birthDate;
        ManagerId = managerId;
        UserId = userId;
        Status = EmployeeStatus.Active;
        TenantId = tenantId;
    }
}
