import { mapEnumToOptions } from '@abp/ng.core';

export enum AttendanceStatus {
  Normal = 1,
  Late = 2,
  EarlyLeave = 3,
  Absent = 4,
  OnLeave = 5,
}

export const attendanceStatusOptions = mapEnumToOptions(AttendanceStatus);
