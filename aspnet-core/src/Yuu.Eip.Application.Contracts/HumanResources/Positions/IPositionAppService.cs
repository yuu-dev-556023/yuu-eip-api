using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Yuu.Eip.HumanResources.Positions;

/// <summary>
/// 職位管理應用服務接口
/// </summary>
public interface IPositionAppService : ICrudAppService<
    PositionDto,
    Guid,
    GetPositionsInput,
    CreateUpdatePositionDto>
{
    /// <summary>
    /// 獲取所有職位列表
    /// </summary>
    /// <returns>所有職位列表</returns>
    Task<List<PositionDto>> GetAllAsync();
}
