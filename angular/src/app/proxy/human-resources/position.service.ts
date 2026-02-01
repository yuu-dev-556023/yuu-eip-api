import type { CreateUpdatePositionDto, GetPositionsInput, PositionDto } from './positions/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PositionService {
  private restService = inject(RestService);
  apiName = 'Eip';
  

  create = (input: CreateUpdatePositionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PositionDto>({
      method: 'POST',
      url: '/api/v1/positions',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/v1/positions/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PositionDto>({
      method: 'GET',
      url: `/api/v1/positions/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GetPositionsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PositionDto>>({
      method: 'GET',
      url: '/api/v1/positions',
      params: { filter: input.filter, level: input.level, isActive: input.isActive, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdatePositionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PositionDto>({
      method: 'PUT',
      url: `/api/v1/positions/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
}