using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;

namespace Yuu.Eip.HumanResources.Employees;

/// <summary>
/// 員工 DTO
/// </summary>
[Description("員工")]
public class EmployeeDto : FullAuditedEntityDto<Guid>
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
    public EmployeeStatus Status { get; set; }

    /// <summary>
    /// 所屬部門 Id
    /// </summary>
    [Description("所屬部門Id")]
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// 所屬部門名稱
    /// </summary>
    [Description("所屬部門名稱")]
    public string? DepartmentName { get; set; }

    /// <summary>
    /// 職位 Id
    /// </summary>
    [Description("職位Id")]
    public Guid PositionId { get; set; }

    /// <summary>
    /// 職位名稱
    /// </summary>
    [Description("職位名稱")]
    public string? PositionName { get; set; }

    /// <summary>
    /// 直屬主管 Id
    /// </summary>
    [Description("直屬主管Id")]
    public Guid? ManagerId { get; set; }

    /// <summary>
    /// 直屬主管名稱
    /// </summary>
    [Description("直屬主管名稱")]
    public string? ManagerName { get; set; }

    /// <summary>
    /// 使用者 Id
    /// </summary>
    [Description("使用者Id")]
    public Guid? UserId { get; set; }
}
