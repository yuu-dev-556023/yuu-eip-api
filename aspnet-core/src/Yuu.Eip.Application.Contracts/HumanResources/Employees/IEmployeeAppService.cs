using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Yuu.Eip.HumanResources.Employees;

/// <summary>
/// 員工管理應用服務接口
/// </summary>
public interface IEmployeeAppService : IApplicationService
{
    /// <summary>
    /// 獲取指定ID的員工資訊
    /// </summary>
    /// <param name="id">員工ID</param>
    /// <returns>員工詳情</returns>
    Task<EmployeeDto> GetAsync(Guid id);

    /// <summary>
    /// 獲取員工列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的員工列表</returns>
    Task<PagedResultDto<EmployeeListDto>> GetListAsync(GetEmployeesInput input);

    /// <summary>
    /// 創建新員工
    /// </summary>
    /// <param name="input">創建員工參數</param>
    /// <returns>創建的員工資訊</returns>
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto input);

    /// <summary>
    /// 更新員工資訊
    /// </summary>
    /// <param name="id">員工ID</param>
    /// <param name="input">更新參數</param>
    /// <returns>更新後的員工資訊</returns>
    Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto input);

    /// <summary>
    /// 刪除員工
    /// </summary>
    /// <param name="id">員工ID</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// 獲取所有員工列表
    /// </summary>
    /// <returns>所有員工列表</returns>
    Task<List<EmployeeListDto>> GetAllAsync();

    /// <summary>
    /// 根據用戶ID獲取員工資訊
    /// </summary>
    /// <param name="userId">用戶ID</param>
    /// <returns>員工資訊，如果不存在則返回null</returns>
    Task<EmployeeDto?> GetByUserIdAsync(Guid userId);
}
