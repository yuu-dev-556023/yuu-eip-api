import type { CreateUpdateDepartmentDto, DepartmentDto, GetDepartmentsInput } from './departments/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private restService = inject(RestService);
  apiName = 'Eip';
  

  create = (input: CreateUpdateDepartmentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DepartmentDto>({
      method: 'POST',
      url: '/api/v1/departments',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/v1/departments/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DepartmentDto>({
      method: 'GET',
      url: `/api/v1/departments/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAll = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DepartmentDto[]>({
      method: 'GET',
      url: '/api/v1/departments/all',
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetDepartmentsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DepartmentDto>>({
      method: 'GET',
      url: '/api/v1/departments',
      params: { filter: input.filter, parentId: input.parentId, isActive: input.isActive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getTree = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DepartmentDto[]>({
      method: 'GET',
      url: '/api/v1/departments/tree',
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateDepartmentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DepartmentDto>({
      method: 'PUT',
      url: `/api/v1/departments/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}