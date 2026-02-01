using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Yuu.Eip.HumanResources.Employees;

/// <summary>
/// 新增員工 DTO
/// </summary>
[Description("新增員工")]
public class CreateEmployeeDto
{
    /// <summary>
    /// 員工編號
    /// </summary>
    [Required]
    [StringLength(HumanResourcesConsts.MaxEmployeeNoLength)]
    [Description("員工編號")]
    public string EmployeeNo { get; set; } = null!;

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
    /// 到職日
    /// </summary>
    [Required]
    [Description("到職日")]
    public DateTime HireDate { get; set; }

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
