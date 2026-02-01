using System;
using System.ComponentModel;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Yuu.Eip.HumanResources;

/// <summary>
/// 打卡記錄
/// </summary>
[Description("打卡記錄")]
public class AttendanceRecord : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// 租戶 Id
    /// </summary>
    [Description("租戶Id")]
    public Guid? TenantId { get; set; }

    /// <summary>
    /// 員工 Id
    /// </summary>
    [Description("員工Id")]
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// 打卡日期
    /// </summary>
    [Description("打卡日期")]
    public DateTime Date { get; set; }

    /// <summary>
    /// 上班打卡時間
    /// </summary>
    [Description("上班打卡時間")]
    public DateTime? ClockIn { get; set; }

    /// <summary>
    /// 下班打卡時間
    /// </summary>
    [Description("下班打卡時間")]
    public DateTime? ClockOut { get; set; }

    /// <summary>
    /// 出勤狀態
    /// </summary>
    [Description("出勤狀態")]
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Normal;

    /// <summary>
    /// 工作時數
    /// </summary>
    [Description("工作時數")]
    public decimal WorkHours { get; set; }

    /// <summary>
    /// 加班時數
    /// </summary>
    [Description("加班時數")]
    public decimal OvertimeHours { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Description("備註")]
    public string? Note { get; set; }

    protected AttendanceRecord()
    {
    }

    public AttendanceRecord(
        Guid id,
        Guid employeeId,
        DateTime date,
        Guid? tenantId = null)
        : base(id)
    {
        EmployeeId = employeeId;
        Date = date.Date;
        Status = AttendanceStatus.Normal;
        TenantId = tenantId;
    }

    /// <summary>
    /// 上班打卡
    /// </summary>
    public void ClockInNow(DateTime clockInTime, TimeSpan workStartTime)
    {
        ClockIn = clockInTime;

        /* 判斷是否遲到 (超過上班時間視為遲到) */
        var expectedStartTime = Date.Add(workStartTime);
        if (clockInTime > expectedStartTime)
        {
            Status = AttendanceStatus.Late;
        }
    }

    /// <summary>
    /// 下班打卡
    /// </summary>
    public void ClockOutNow(DateTime clockOutTime, TimeSpan workEndTime, decimal standardWorkHours = 8m)
    {
        ClockOut = clockOutTime;

        /* 計算工作時數 */
        if (ClockIn.HasValue)
        {
            var totalHours = (decimal)(clockOutTime - ClockIn.Value).TotalHours;
            WorkHours = Math.Max(0, totalHours);

            /* 計算加班時數 (超過標準工時) */
            if (WorkHours > standardWorkHours)
            {
                OvertimeHours = WorkHours - standardWorkHours;
            }
        }

        /* 判斷是否早退 (未到下班時間視為早退) */
        var expectedEndTime = Date.Add(workEndTime);
        if (clockOutTime < expectedEndTime && Status != AttendanceStatus.Late)
        {
            Status = AttendanceStatus.EarlyLeave;
        }
        else if (clockOutTime < expectedEndTime && Status == AttendanceStatus.Late)
        {
            /* 遲到又早退仍標記為遲到 */
        }
    }

    /// <summary>
    /// 標記為請假
    /// </summary>
    public void MarkAsOnLeave()
    {
        Status = AttendanceStatus.OnLeave;
    }

    /// <summary>
    /// 標記為曠職
    /// </summary>
    public void MarkAsAbsent()
    {
        Status = AttendanceStatus.Absent;
    }
}
