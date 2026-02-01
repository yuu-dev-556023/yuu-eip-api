using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;
using Yuu.Eip.Controllers;
using Yuu.Eip.HumanResources.Departments;

namespace Yuu.Eip.HumanResources;

[SwaggerTag("部門管理")]
public class DepartmentController : EipController
{
    private readonly IDepartmentAppService _departmentAppService;

    /// <summary>
    /// 初始化部門控制器
    /// </summary>
    /// <param name="departmentAppService">部門應用服務</param>
    public DepartmentController(IDepartmentAppService departmentAppService)
    {
        _departmentAppService = departmentAppService;
    }

    /// <summary>
    /// 獲取部門列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的部門列表</returns>
    [HttpGet]
    public Task<PagedResultDto<DepartmentDto>> GetListAsync([FromQuery] GetDepartmentsInput input)
    {
        return _departmentAppService.GetListAsync(input);
    }

    /// <summary>
    /// 獲取指定ID的部門
    /// </summary>
    /// <param name="id">部門ID</param>
    /// <returns>部門詳情</returns>
    [HttpGet("{id}")]
    public Task<DepartmentDto> GetAsync(Guid id)
    {
        return _departmentAppService.GetAsync(id);
    }

    /// <summary>
    /// 創建新部門
    /// </summary>
    /// <param name="input">部門創建參數</param>
    /// <returns>創建的部門詳情</returns>
    [HttpPost]
    public Task<DepartmentDto> CreateAsync([FromBody] CreateUpdateDepartmentDto input)
    {
        return _departmentAppService.CreateAsync(input);
    }

    /// <summary>
    /// 更新部門信息
    /// </summary>
    /// <param name="id">部門ID</param>
    /// <param name="input">部門更新參數</param>
    /// <returns>更新後的部門詳情</returns>
    [HttpPut("{id}")]
    public Task<DepartmentDto> UpdateAsync(Guid id, [FromBody] CreateUpdateDepartmentDto input)
    {
        return _departmentAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 刪除部門
    /// </summary>
    /// <param name="id">部門ID</param>
    /// <returns>刪除操作結果</returns>
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _departmentAppService.DeleteAsync(id);
    }

    /// <summary>
    /// 獲取部門樹狀結構
    /// </summary>
    /// <returns>部門樹狀結構列表</returns>
    [HttpGet("tree")]
    public Task<List<DepartmentDto>> GetTreeAsync()
    {
        return _departmentAppService.GetTreeAsync();
    }

    /// <summary>
    /// 獲取所有部門
    /// </summary>
    /// <returns>所有部門列表</returns>
    [HttpGet("all")]
    public Task<List<DepartmentDto>> GetAllAsync()
    {
        return _departmentAppService.GetAllAsync();
    }
}
