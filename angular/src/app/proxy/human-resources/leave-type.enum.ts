import { mapEnumToOptions } from '@abp/ng.core';

export enum LeaveType {
  Annual = 1,
  Sick = 2,
  Personal = 3,
  Maternity = 4,
  Paternity = 5,
  Unpaid = 6,
}

export const leaveTypeOptions = mapEnumToOptions(LeaveType);
