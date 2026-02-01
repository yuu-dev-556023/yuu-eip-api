using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Yuu.Eip.HumanResources.Salaries;

/// <summary>
/// 薪資管理應用服務接口
/// </summary>
public interface ISalaryAppService : IApplicationService
{
    /// <summary>
    /// 獲取指定ID的薪資記錄
    /// </summary>
    /// <param name="id">薪資記錄ID</param>
    /// <returns>薪資記錄詳情</returns>
    Task<SalaryRecordDto> GetAsync(Guid id);

    /// <summary>
    /// 獲取薪資記錄列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的薪資記錄列表</returns>
    Task<PagedResultDto<SalaryRecordDto>> GetListAsync(GetSalaryRecordsInput input);

    /// <summary>
    /// 計算薪資
    /// </summary>
    /// <param name="input">計算薪資參數</param>
    /// <returns>計算後的薪資記錄</returns>
    Task<SalaryRecordDto> CalculateAsync(CalculateSalaryInput input);

    /// <summary>
    /// 批量計算薪資
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="departmentId">部門ID，可選</param>
    /// <returns>批量計算的薪資記錄列表</returns>
    Task<List<SalaryRecordDto>> BatchCalculateAsync(int year, int month, Guid? departmentId = null);

    /// <summary>
    /// 確認薪資記錄
    /// </summary>
    /// <param name="id">薪資記錄ID</param>
    /// <returns>確認後的薪資記錄</returns>
    Task<SalaryRecordDto> ConfirmAsync(Guid id);

    /// <summary>
    /// 標記薪資記錄為已支付
    /// </summary>
    /// <param name="id">薪資記錄ID</param>
    /// <returns>標記為已支付的薪資記錄</returns>
    Task<SalaryRecordDto> MarkAsPaidAsync(Guid id);

    /// <summary>
    /// 獲取薪資單
    /// </summary>
    /// <param name="id">薪資記錄ID</param>
    /// <returns>薪資單詳情</returns>
    Task<SalarySlipDto> GetSalarySlipAsync(Guid id);

    /// <summary>
    /// 獲取我的薪資單
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <returns>我的薪資單詳情，如果不存在則返回null</returns>
    Task<SalarySlipDto?> GetMySalarySlipAsync(int year, int month);
}
