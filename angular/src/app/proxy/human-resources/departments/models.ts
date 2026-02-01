import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreateUpdateDepartmentDto {
  code: string;
  name: string;
  parentId?: string | null;
  managerId?: string | null;
  order?: number;
  isActive?: boolean;
}

export interface DepartmentDto extends FullAuditedEntityDto<string> {
  code?: string;
  name?: string;
  parentId?: string | null;
  parentName?: string | null;
  managerId?: string | null;
  managerName?: string | null;
  order?: number;
  isActive?: boolean;
}

export interface GetDepartmentsInput extends PagedAndSortedResultRequestDto {
  filter?: string | null;
  parentId?: string | null;
  isActive?: boolean | null;
}
