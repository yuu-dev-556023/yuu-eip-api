namespace Yuu.Eip.HumanResources;

/// <summary>
/// 人資模組常數
/// </summary>
public static class HumanResourcesConsts
{
    /// <summary>
    /// 部門代碼最大長度
    /// </summary>
    public const int MaxDepartmentCodeLength = 32;

    /// <summary>
    /// 部門名稱最大長度
    /// </summary>
    public const int MaxDepartmentNameLength = 128;

    /// <summary>
    /// 職位代碼最大長度
    /// </summary>
    public const int MaxPositionCodeLength = 32;

    /// <summary>
    /// 職位名稱最大長度
    /// </summary>
    public const int MaxPositionNameLength = 128;

    /// <summary>
    /// 員工編號最大長度
    /// </summary>
    public const int MaxEmployeeNoLength = 32;

    /// <summary>
    /// 員工姓名最大長度
    /// </summary>
    public const int MaxEmployeeNameLength = 64;

    /// <summary>
    /// Email 最大長度
    /// </summary>
    public const int MaxEmailLength = 256;

    /// <summary>
    /// 電話最大長度
    /// </summary>
    public const int MaxPhoneLength = 32;

    /// <summary>
    /// 請假事由最大長度
    /// </summary>
    public const int MaxLeaveReasonLength = 500;

    /// <summary>
    /// 審核意見最大長度
    /// </summary>
    public const int MaxApproverCommentLength = 500;

    /// <summary>
    /// 備註最大長度
    /// </summary>
    public const int MaxNoteLength = 500;

    /// <summary>
    /// 每月標準工時 (8小時 * 30天)
    /// </summary>
    public const decimal StandardMonthlyWorkHours = 240m;

    /// <summary>
    /// 加班費率 (1.33倍)
    /// </summary>
    public const decimal OvertimeRate = 1.33m;

    /// <summary>
    /// 勞保費率 (10.5%)
    /// </summary>
    public const decimal LaborInsuranceRate = 0.105m;

    /// <summary>
    /// 健保費率 (5.17%)
    /// </summary>
    public const decimal HealthInsuranceRate = 0.0517m;
}
