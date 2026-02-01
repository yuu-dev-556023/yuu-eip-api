import { mapEnumToOptions } from '@abp/ng.core';

export enum EmployeeStatus {
  Active = 1,
  Resigned = 2,
  OnLeave = 3,
}

export const employeeStatusOptions = mapEnumToOptions(EmployeeStatus);
