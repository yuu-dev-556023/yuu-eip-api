import type { CreateEmployeeDto, EmployeeDto, EmployeeListDto, GetEmployeesInput, UpdateEmployeeDto } from './employees/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private restService = inject(RestService);
  apiName = 'Eip';
  

  create = (input: CreateEmployeeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeDto>({
      method: 'POST',
      url: '/api/v1/employees',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/v1/employees/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeDto>({
      method: 'GET',
      url: `/api/v1/employees/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeListDto[]>({
      method: 'GET',
      url: '/api/v1/employees/all',
    },
    { apiName: this.apiName,...config });
  

  getByUserId = (userId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeDto>({
      method: 'GET',
      url: `/api/v1/employees/by-user/${userId}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetEmployeesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<EmployeeListDto>>({
      method: 'GET',
      url: '/api/v1/employees',
      params: { filter: input.filter, departmentId: input.departmentId, positionId: input.positionId, status: input.status, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateEmployeeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, EmployeeDto>({
      method: 'PUT',
      url: `/api/v1/employees/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}