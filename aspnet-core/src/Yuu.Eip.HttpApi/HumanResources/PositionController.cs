using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;
using Yuu.Eip.Controllers;
using Yuu.Eip.HumanResources.Positions;

namespace Yuu.Eip.HumanResources;

[SwaggerTag("職位管理")]
public class PositionController : EipController
{
    private readonly IPositionAppService _positionAppService;

    /// <summary>
    /// 初始化職位控制器
    /// </summary>
    /// <param name="positionAppService">職位應用服務</param>
    public PositionController(IPositionAppService positionAppService)
    {
        _positionAppService = positionAppService;
    }

    /// <summary>
    /// 獲取職位列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的職位列表</returns>
    [HttpGet]
    public Task<PagedResultDto<PositionDto>> GetListAsync([FromQuery] GetPositionsInput input)
    {
        return _positionAppService.GetListAsync(input);
    }

    /// <summary>
    /// 獲取指定ID的職位
    /// </summary>
    /// <param name="id">職位ID</param>
    /// <returns>職位詳情</returns>
    [HttpGet("{id}")]
    public Task<PositionDto> GetAsync(Guid id)
    {
        return _positionAppService.GetAsync(id);
    }

    /// <summary>
    /// 創建新職位
    /// </summary>
    /// <param name="input">職位創建參數</param>
    /// <returns>創建的職位詳情</returns>
    [HttpPost]
    public Task<PositionDto> CreateAsync([FromBody] CreateUpdatePositionDto input)
    {
        return _positionAppService.CreateAsync(input);
    }

    /// <summary>
    /// 更新職位信息
    /// </summary>
    /// <param name="id">職位ID</param>
    /// <param name="input">職位更新參數</param>
    /// <returns>更新後的職位詳情</returns>
    [HttpPut("{id}")]
    public Task<PositionDto> UpdateAsync(Guid id, [FromBody] CreateUpdatePositionDto input)
    {
        return _positionAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 刪除職位
    /// </summary>
    /// <param name="id">職位ID</param>
    /// <returns>刪除操作結果</returns>
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _positionAppService.DeleteAsync(id);
    }
}
