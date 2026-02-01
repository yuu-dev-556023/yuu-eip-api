import type { Gender } from '../gender.enum';
import type { EntityDto, FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { EmployeeStatus } from '../employee-status.enum';

export interface CreateEmployeeDto {
  employeeNo: string;
  name: string;
  email?: string | null;
  phone?: string | null;
  birthDate?: string | null;
  gender: Gender;
  hireDate: string;
  departmentId: string;
  positionId: string;
  managerId?: string | null;
  userId?: string | null;
}

export interface EmployeeDto extends FullAuditedEntityDto<string> {
  employeeNo?: string;
  name?: string;
  email?: string | null;
  phone?: string | null;
  birthDate?: string | null;
  gender?: Gender;
  hireDate?: string;
  terminationDate?: string | null;
  status?: EmployeeStatus;
  departmentId?: string;
  departmentName?: string | null;
  positionId?: string;
  positionName?: string | null;
  managerId?: string | null;
  managerName?: string | null;
  userId?: string | null;
}

export interface EmployeeListDto extends EntityDto<string> {
  employeeNo?: string;
  name?: string;
  email?: string | null;
  phone?: string | null;
  status?: EmployeeStatus;
  departmentName?: string | null;
  positionName?: string | null;
  hireDate?: string;
}

export interface GetEmployeesInput extends PagedAndSortedResultRequestDto {
  filter?: string | null;
  departmentId?: string | null;
  positionId?: string | null;
  status?: EmployeeStatus | null;
}

export interface UpdateEmployeeDto {
  name: string;
  email?: string | null;
  phone?: string | null;
  birthDate?: string | null;
  gender: Gender;
  terminationDate?: string | null;
  status: EmployeeStatus;
  departmentId: string;
  positionId: string;
  managerId?: string | null;
  userId?: string | null;
}
