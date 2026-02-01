using System;
using System.ComponentModel;

namespace Yuu.Eip.HumanResources.Attendance;

/// <summary>
/// 簽到簽退 DTO
/// </summary>
[Description("簽到簽退")]
public class ClockInOutDto
{
    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    public Guid? EmployeeId { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Description("備註")]
    public string? Note { get; set; }
}
