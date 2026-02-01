using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;
using Yuu.Eip.Controllers;
using Yuu.Eip.HumanResources.LeaveRequests;

namespace Yuu.Eip.HumanResources;

[SwaggerTag("請假管理")]
public class LeaveRequestController : EipController
{
    private readonly ILeaveRequestAppService _leaveRequestAppService;

    /// <summary>
    /// 初始化請假控制器
    /// </summary>
    /// <param name="leaveRequestAppService">請假應用服務</param>
    public LeaveRequestController(ILeaveRequestAppService leaveRequestAppService)
    {
        _leaveRequestAppService = leaveRequestAppService;
    }

    /// <summary>
    /// 獲取請假申請列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的請假申請列表</returns>
    [HttpGet]
    public Task<PagedResultDto<LeaveRequestDto>> GetListAsync([FromQuery] GetLeaveRequestsInput input)
    {
        return _leaveRequestAppService.GetListAsync(input);
    }

    /// <summary>
    /// 獲取指定ID的請假申請
    /// </summary>
    /// <param name="id">請假申請ID</param>
    /// <returns>請假申請詳情</returns>
    [HttpGet("{id}")]
    public Task<LeaveRequestDto> GetAsync(Guid id)
    {
        return _leaveRequestAppService.GetAsync(id);
    }

    /// <summary>
    /// 創建請假申請
    /// </summary>
    /// <param name="input">請假申請參數</param>
    /// <returns>創建的請假申請詳情</returns>
    [HttpPost]
    public Task<LeaveRequestDto> CreateAsync([FromBody] CreateLeaveRequestDto input)
    {
        return _leaveRequestAppService.CreateAsync(input);
    }

    /// <summary>
    /// 審批通過請假申請
    /// </summary>
    /// <param name="id">請假申請ID</param>
    /// <param name="input">審批參數</param>
    /// <returns>審批後的請假申請詳情</returns>
    [HttpPost("{id}/approve")]
    public Task ApproveAsync(Guid id, [FromBody] ApproveRejectLeaveRequestDto input)
    {
        return _leaveRequestAppService.ApproveAsync(id, input);
    }

    /// <summary>
    /// 審批拒絕請假申請
    /// </summary>
    /// <param name="id">請假申請ID</param>
    /// <param name="input">審批參數</param>
    /// <returns>審批後的請假申請詳情</returns>
    [HttpPost("{id}/reject")]
    public Task RejectAsync(Guid id, [FromBody] ApproveRejectLeaveRequestDto input)
    {
        return _leaveRequestAppService.RejectAsync(id, input);
    }

    /// <summary>
    /// 取消請假申請
    /// </summary>
    /// <param name="id">請假申請ID</param>
    /// <returns>取消後的請假申請詳情</returns>
    [HttpPost("{id}/cancel")]
    public Task CancelAsync(Guid id)
    {
        return _leaveRequestAppService.CancelAsync(id);
    }

    /// <summary>
    /// 獲取當前用戶的請假申請
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>當前用戶的請假申請列表</returns>
    [HttpGet("my")]
    public Task<PagedResultDto<LeaveRequestDto>> GetMyLeaveRequestsAsync([FromQuery] GetLeaveRequestsInput input)
    {
        return _leaveRequestAppService.GetMyLeaveRequestsAsync(input);
    }

    /// <summary>
    /// 獲取待審批的請假申請
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>待審批的請假申請列表</returns>
    [HttpGet("pending-approvals")]
    public Task<PagedResultDto<LeaveRequestDto>> GetPendingApprovalsAsync([FromQuery] GetLeaveRequestsInput input)
    {
        return _leaveRequestAppService.GetPendingApprovalsAsync(input);
    }
}
