import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateUpdatePositionDto {
  code: string;
  name: string;
  level?: number;
  baseSalary?: number;
  isActive?: boolean;
}

export interface GetPositionsInput extends PagedAndSortedResultRequestDto {
  filter?: string | null;
  level?: number | null;
  isActive?: boolean | null;
}

export interface PositionDto extends FullAuditedEntityDto<string> {
  code?: string;
  name?: string;
  level?: number;
  baseSalary?: number;
  isActive?: boolean;
}
