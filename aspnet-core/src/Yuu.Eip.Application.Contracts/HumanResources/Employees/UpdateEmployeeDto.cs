using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yuu.Eip.HumanResources.Employees;

/// <summary>
/// 更新員工 DTO
/// </summary>
[Description("更新員工")]
public class UpdateEmployeeDto
{
    /// <summary>
    /// 姓名
    /// </summary>
    [Required]
    [StringLength(HumanResourcesConsts.MaxEmployeeNameLength)]
    [Description("姓名")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 電子郵件
    /// </summary>
    [EmailAddress]
    [StringLength(HumanResourcesConsts.MaxEmailLength)]
    [Description("電子郵件")]
    public string? Email { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    [StringLength(HumanResourcesConsts.MaxPhoneLength)]
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
    [Required]
    [Description("性別")]
    public Gender Gender { get; set; }

    /// <summary>
    /// 離職日
    /// </summary>
    [Description("離職日")]
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// 員工狀態
    /// </summary>
    [Required]
    [Description("員工狀態")]
    public EmployeeStatus Status { get; set; }

    /// <summary>
    /// 所屬部門 Id
    /// </summary>
    [Required]
    [Description("所屬部門Id")]
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// 職位 Id
    /// </summary>
    [Required]
    [Description("職位Id")]
    public Guid PositionId { get; set; }

    /// <summary>
    /// 直屬主管 Id
    /// </summary>
    [Description("直屬主管Id")]
    public Guid? ManagerId { get; set; }

    /// <summary>
    /// 使用者 Id
    /// </summary>
    [Description("使用者Id")]
    public Guid? UserId { get; set; }
}
