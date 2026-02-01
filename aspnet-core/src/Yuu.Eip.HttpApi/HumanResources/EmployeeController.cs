using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;
using Yuu.Eip.Controllers;
using Yuu.Eip.HumanResources.Employees;

namespace Yuu.Eip.HumanResources;

[SwaggerTag("員工管理")]
public class EmployeeController : EipController
{
    private readonly IEmployeeAppService _employeeAppService;

    /// <summary>
    /// 初始化員工控制器
    /// </summary>
    /// <param name="employeeAppService">員工應用服務</param>
    public EmployeeController(IEmployeeAppService employeeAppService)
    {
        _employeeAppService = employeeAppService;
    }

    /// <summary>
    /// 獲取員工列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的員工列表</returns>
    [HttpGet]
    public Task<PagedResultDto<EmployeeListDto>> GetListAsync([FromQuery] GetEmployeesInput input)
    {
        return _employeeAppService.GetListAsync(input);
    }

    /// <summary>
    /// 獲取指定ID的員工
    /// </summary>
    /// <param name="id">員工ID</param>
    /// <returns>員工詳情</returns>
    [HttpGet("{id}")]
    public Task<EmployeeDto> GetAsync(Guid id)
    {
        return _employeeAppService.GetAsync(id);
    }

    /// <summary>
    /// 創建新員工
    /// </summary>
    /// <param name="input">員工創建參數</param>
    /// <returns>創建的員工詳情</returns>
    [HttpPost]
    public Task<EmployeeDto> CreateAsync([FromBody] CreateEmployeeDto input)
    {
        return _employeeAppService.CreateAsync(input);
    }

    /// <summary>
    /// 更新員工信息
    /// </summary>
    /// <param name="id">員工ID</param>
    /// <param name="input">員工更新參數</param>
    /// <returns>更新後的員工詳情</returns>
    [HttpPut("{id}")]
    public Task<EmployeeDto> UpdateAsync(Guid id, [FromBody] UpdateEmployeeDto input)
    {
        return _employeeAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 刪除員工
    /// </summary>
    /// <param name="id">員工ID</param>
    /// <returns>刪除操作結果</returns>
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _employeeAppService.DeleteAsync(id);
    }

    /// <summary>
    /// 獲取所有員工
    /// </summary>
    /// <returns>所有員工列表</returns>
    [HttpGet("all")]
    public Task<List<EmployeeListDto>> GetAllAsync()
    {
        return _employeeAppService.GetAllAsync();
    }

    /// <summary>
    /// 根據用戶ID獲取員工信息
    /// </summary>
    /// <param name="userId">用戶ID</param>
    /// <returns>員工信息</returns>
    [HttpGet("by-user/{userId}")]
    public Task<EmployeeDto?> GetByUserIdAsync(Guid userId)
    {
        return _employeeAppService.GetByUserIdAsync(userId);
    }
}
