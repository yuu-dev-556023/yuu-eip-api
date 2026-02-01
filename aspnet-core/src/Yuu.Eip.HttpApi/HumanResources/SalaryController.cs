using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;
using Yuu.Eip.Controllers;
using Yuu.Eip.HumanResources.Salaries;

namespace Yuu.Eip.HumanResources;

[SwaggerTag("薪資管理")]
public class SalaryController : EipController
{
    private readonly ISalaryAppService _salaryAppService;

    /// <summary>
    /// 初始化薪資控制器
    /// </summary>
    /// <param name="salaryAppService">薪資應用服務</param>
    public SalaryController(ISalaryAppService salaryAppService)
    {
        _salaryAppService = salaryAppService;
    }

    /// <summary>
    /// 獲取薪資記錄列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的薪資記錄列表</returns>
    [HttpGet]
    public Task<PagedResultDto<SalaryRecordDto>> GetListAsync([FromQuery] GetSalaryRecordsInput input)
    {
        return _salaryAppService.GetListAsync(input);
    }

    /// <summary>
    /// 獲取指定ID的薪資記錄
    /// </summary>
    /// <param name="id">薪資記錄ID</param>
    /// <returns>薪資記錄詳情</returns>
    [HttpGet("{id}")]
    public Task<SalaryRecordDto> GetAsync(Guid id)
    {
        return _salaryAppService.GetAsync(id);
    }

    /// <summary>
    /// 計算薪資
    /// </summary>
    /// <param name="input">薪資計算參數</param>
    /// <returns>計算後的薪資記錄</returns>
    [HttpPost("calculate")]
    public Task<SalaryRecordDto> CalculateAsync([FromBody] CalculateSalaryInput input)
    {
        return _salaryAppService.CalculateAsync(input);
    }

    /// <summary>
    /// 確認薪資記錄
    /// </summary>
    /// <param name="id">薪資記錄ID</param>
    /// <returns>確認後的薪資記錄</returns>
    [HttpPost("{id}/confirm")]
    public Task ConfirmAsync(Guid id)
    {
        return _salaryAppService.ConfirmAsync(id);
    }

    /// <summary>
    /// 標記薪資為已支付
    /// </summary>
    /// <param name="id">薪資記錄ID</param>
    /// <returns>標記後的薪資記錄</returns>
    [HttpPost("{id}/mark-as-paid")]
    public Task MarkAsPaidAsync(Guid id)
    {
        return _salaryAppService.MarkAsPaidAsync(id);
    }

    /// <summary>
    /// 獲取薪資單
    /// </summary>
    /// <param name="id">薪資記錄ID</param>
    /// <returns>薪資單詳情</returns>
    [HttpGet("{id}/slip")]
    public Task<SalarySlipDto> GetSalarySlipAsync(Guid id)
    {
        return _salaryAppService.GetSalarySlipAsync(id);
    }

    /// <summary>
    /// 獲取當前用戶的薪資單
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <returns>當前用戶的薪資單詳情</returns>
    [HttpGet("my-slip")]
    public Task<SalarySlipDto?> GetMySalarySlipAsync([FromQuery] int year, [FromQuery] int month)
    {
        return _salaryAppService.GetMySalarySlipAsync(year, month);
    }

    /// <summary>
    /// 批量計算薪資
    /// </summary>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <param name="departmentId">部門ID，可選</param>
    /// <returns>批量計算的薪資記錄列表</returns>
    [HttpPost("batch-calculate")]
    public Task<List<SalaryRecordDto>> BatchCalculateAsync([FromQuery] int year, [FromQuery] int month, [FromQuery] Guid? departmentId = null)
    {
        return _salaryAppService.BatchCalculateAsync(year, month, departmentId);
    }
}
