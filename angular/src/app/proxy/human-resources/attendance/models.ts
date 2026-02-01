import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { AttendanceStatus } from '../attendance-status.enum';

export interface AttendanceRecordDto extends FullAuditedEntityDto<string> {
  employeeId?: string;
  employeeName?: string | null;
  employeeNo?: string | null;
  date?: string;
  clockIn?: string | null;
  clockOut?: string | null;
  status?: AttendanceStatus;
  workHours?: number;
  overtimeHours?: number;
  note?: string | null;
}

export interface AttendanceReportDto {
  employeeId?: string;
  employeeName?: string | null;
  employeeNo?: string | null;
  year?: number;
  month?: number;
  workDays?: number;
  actualWorkDays?: number;
  lateDays?: number;
  earlyLeaveDays?: number;
  absentDays?: number;
  leaveDays?: number;
  totalWorkHours?: number;
  totalOvertimeHours?: number;
}

export interface ClockInOutDto {
  employeeId?: string | null;
  note?: string | null;
}

export interface GetAttendanceInput extends PagedAndSortedResultRequestDto {
  employeeId?: string | null;
  departmentId?: string | null;
  dateFrom?: string | null;
  dateTo?: string | null;
  status?: AttendanceStatus | null;
}

export interface UpdateAttendanceRecordDto {
  clockIn?: string | null;
  clockOut?: string | null;
  status?: AttendanceStatus;
  workHours?: number;
  overtimeHours?: number;
  note?: string | null;
}
