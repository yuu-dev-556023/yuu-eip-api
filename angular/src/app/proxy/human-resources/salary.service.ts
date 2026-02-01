import type { CalculateSalaryInput, GetSalaryRecordsInput, SalaryRecordDto, SalarySlipDto } from './salaries/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SalaryService {
  private restService = inject(RestService);
  apiName = 'Eip';
  

  batchCalculate = (year: number, month: number, departmentId?: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalaryRecordDto[]>({
      method: 'POST',
      url: '/api/v1/salaries/batch-calculate',
      params: { year, month, departmentId },
    },
    { apiName: this.apiName,...config });
  

  calculate = (input: CalculateSalaryInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalaryRecordDto>({
      method: 'POST',
      url: '/api/v1/salaries/calculate',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  confirm = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/v1/salaries/${id}/confirm`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalaryRecordDto>({
      method: 'GET',
      url: `/api/v1/salaries/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetSalaryRecordsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SalaryRecordDto>>({
      method: 'GET',
      url: '/api/v1/salaries',
      params: { employeeId: input.employeeId, departmentId: input.departmentId, year: input.year, month: input.month, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getMySalarySlip = (year: number, month: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalarySlipDto>({
      method: 'GET',
      url: '/api/v1/salaries/my-slip',
      params: { year, month },
    },
    { apiName: this.apiName,...config });
  

  getSalarySlip = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalarySlipDto>({
      method: 'GET',
      url: `/api/v1/salaries/${id}/slip`,
    },
    { apiName: this.apiName,...config });
  

  markAsPaid = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/v1/salaries/${id}/mark-as-paid`,
    },
    { apiName: this.apiName,...config });
}