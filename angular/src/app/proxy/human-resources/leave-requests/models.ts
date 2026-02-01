import type { LeaveType } from '../leave-type.enum';
import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { LeaveRequestStatus } from '../leave-request-status.enum';

export interface ApproveRejectLeaveRequestDto {
  comment?: string | null;
}

export interface CreateLeaveRequestDto {
  employeeId: string;
  leaveType: LeaveType;
  startDate: string;
  endDate: string;
  hours: number;
  reason?: string | null;
}

export interface GetLeaveRequestsInput extends PagedAndSortedResultRequestDto {
  employeeId?: string | null;
  leaveType?: LeaveType | null;
  status?: LeaveRequestStatus | null;
  startDateFrom?: string | null;
  startDateTo?: string | null;
}

export interface LeaveRequestDto extends FullAuditedEntityDto<string> {
  employeeId?: string;
  employeeName?: string | null;
  employeeNo?: string | null;
  leaveType?: LeaveType;
  startDate?: string;
  endDate?: string;
  hours?: number;
  reason?: string | null;
  status?: LeaveRequestStatus;
  approverId?: string | null;
  approverName?: string | null;
  approvedTime?: string | null;
  approverComment?: string | null;
}
