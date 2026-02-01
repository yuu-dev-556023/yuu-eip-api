using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Yuu.Eip.HumanResources.Departments;

/// <summary>
/// 部門管理應用服務接口
/// </summary>
public interface IDepartmentAppService : ICrudAppService<
    DepartmentDto,
    Guid,
    GetDepartmentsInput,
    CreateUpdateDepartmentDto>
{
    /// <summary>
    /// 獲取所有部門列表
    /// </summary>
    /// <returns>部門列表</returns>
    Task<List<DepartmentDto>> GetAllAsync();

    /// <summary>
    /// 獲取部門樹狀結構
    /// </summary>
    /// <returns>樹狀結構的部門列表</returns>
    Task<List<DepartmentDto>> GetTreeAsync();
}
