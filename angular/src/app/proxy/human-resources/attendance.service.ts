import type { AttendanceRecordDto, AttendanceReportDto, ClockInOutDto, GetAttendanceInput, UpdateAttendanceRecordDto } from './attendance/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private restService = inject(RestService);
  apiName = 'Eip';
  

  clockIn = (input: ClockInOutDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendanceRecordDto>({
      method: 'POST',
      url: '/api/v1/attendances/clock-in',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  clockOut = (input: ClockInOutDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendanceRecordDto>({
      method: 'POST',
      url: '/api/v1/attendances/clock-out',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendanceRecordDto>({
      method: 'GET',
      url: `/api/v1/attendances/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetAttendanceInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AttendanceRecordDto>>({
      method: 'GET',
      url: '/api/v1/attendances',
      params: { employeeId: input.employeeId, departmentId: input.departmentId, dateFrom: input.dateFrom, dateTo: input.dateTo, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getMonthlyReport = (year: number, month: number, departmentId?: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendanceReportDto[]>({
      method: 'GET',
      url: '/api/v1/attendances/monthly-report',
      params: { year, month, departmentId },
    },
    { apiName: this.apiName,...config });
  

  getTodayRecord = (employeeId?: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendanceRecordDto>({
      method: 'GET',
      url: '/api/v1/attendances/today',
      params: { employeeId },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateAttendanceRecordDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AttendanceRecordDto>({
      method: 'PUT',
      url: `/api/v1/attendances/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}