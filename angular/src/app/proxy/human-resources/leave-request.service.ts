import type { ApproveRejectLeaveRequestDto, CreateLeaveRequestDto, GetLeaveRequestsInput, LeaveRequestDto } from './leave-requests/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LeaveRequestService {
  private restService = inject(RestService);
  apiName = 'Eip';
  

  approve = (id: string, input: ApproveRejectLeaveRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/v1/leave-requests/${id}/approve`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  cancel = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/v1/leave-requests/${id}/cancel`,
    },
    { apiName: this.apiName,...config });
  

  create = (input: CreateLeaveRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LeaveRequestDto>({
      method: 'POST',
      url: '/api/v1/leave-requests',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LeaveRequestDto>({
      method: 'GET',
      url: `/api/v1/leave-requests/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetLeaveRequestsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<LeaveRequestDto>>({
      method: 'GET',
      url: '/api/v1/leave-requests',
      params: { employeeId: input.employeeId, leaveType: input.leaveType, status: input.status, startDateFrom: input.startDateFrom, startDateTo: input.startDateTo, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getMyLeaveRequests = (input: GetLeaveRequestsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<LeaveRequestDto>>({
      method: 'GET',
      url: '/api/v1/leave-requests/my',
      params: { employeeId: input.employeeId, leaveType: input.leaveType, status: input.status, startDateFrom: input.startDateFrom, startDateTo: input.startDateTo, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getPendingApprovals = (input: GetLeaveRequestsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<LeaveRequestDto>>({
      method: 'GET',
      url: '/api/v1/leave-requests/pending-approvals',
      params: { employeeId: input.employeeId, leaveType: input.leaveType, status: input.status, startDateFrom: input.startDateFrom, startDateTo: input.startDateTo, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  reject = (id: string, input: ApproveRejectLeaveRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/v1/leave-requests/${id}/reject`,
      body: input,
    },
    { apiName: this.apiName,...config });
}