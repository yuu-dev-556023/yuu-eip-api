using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Yuu.Eip.HumanResources.LeaveRequests;

/// <summary>
/// 請假申請應用服務接口
/// </summary>
public interface ILeaveRequestAppService : IApplicationService
{
    /// <summary>
    /// 獲取指定ID的請假申請
    /// </summary>
    /// <param name="id">請假申請ID</param>
    /// <returns>請假申請詳情</returns>
    Task<LeaveRequestDto> GetAsync(Guid id);

    /// <summary>
    /// 獲取請假申請列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的請假申請列表</returns>
    Task<PagedResultDto<LeaveRequestDto>> GetListAsync(GetLeaveRequestsInput input);

    /// <summary>
    /// 創建請假申請
    /// </summary>
    /// <param name="input">創建請假申請參數</param>
    /// <returns>創建的請假申請</returns>
    Task<LeaveRequestDto> CreateAsync(CreateLeaveRequestDto input);

    /// <summary>
    /// 刪除請假申請
    /// </summary>
    /// <param name="id">請假申請ID</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// 批准請假申請
    /// </summary>
    /// <param name="id">請假申請ID</param>
    /// <param name="input">批准參數</param>
    /// <returns>批准後的請假申請</returns>
    Task<LeaveRequestDto> ApproveAsync(Guid id, ApproveRejectLeaveRequestDto input);

    /// <summary>
    /// 拒絕請假申請
    /// </summary>
    /// <param name="id">請假申請ID</param>
    /// <param name="input">拒絕參數</param>
    /// <returns>拒絕後的請假申請</returns>
    Task<LeaveRequestDto> RejectAsync(Guid id, ApproveRejectLeaveRequestDto input);

    /// <summary>
    /// 取消請假申請
    /// </summary>
    /// <param name="id">請假申請ID</param>
    /// <returns>取消後的請假申請</returns>
    Task<LeaveRequestDto> CancelAsync(Guid id);

    /// <summary>
    /// 獲取我的請假申請列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的我的請假申請列表</returns>
    Task<PagedResultDto<LeaveRequestDto>> GetMyLeaveRequestsAsync(GetLeaveRequestsInput input);

    /// <summary>
    /// 獲取待審批的請假申請列表
    /// </summary>
    /// <param name="input">查詢參數</param>
    /// <returns>分頁的待審批請假申請列表</returns>
    Task<PagedResultDto<LeaveRequestDto>> GetPendingApprovalsAsync(GetLeaveRequestsInput input);
}
