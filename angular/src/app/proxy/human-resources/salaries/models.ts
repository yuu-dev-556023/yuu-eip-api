import type { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { SalaryStatus } from '../salary-status.enum';

export interface CalculateSalaryInput {
  employeeId: string;
  year: number;
  month: number;
  bonus?: number;
}

export interface GetSalaryRecordsInput extends PagedAndSortedResultRequestDto {
  employeeId?: string | null;
  departmentId?: string | null;
  year?: number | null;
  month?: number | null;
  status?: SalaryStatus | null;
}

export interface SalaryRecordDto extends FullAuditedEntityDto<string> {
  employeeId?: string;
  employeeName?: string | null;
  employeeNo?: string | null;
  year?: number;
  month?: number;
  baseSalary?: number;
  overtimePay?: number;
  bonus?: number;
  deductions?: number;
  laborInsurance?: number;
  healthInsurance?: number;
  netSalary?: number;
  status?: SalaryStatus;
  note?: string | null;
}

export interface SalarySlipDto {
  employeeId?: string;
  employeeName?: string | null;
  employeeNo?: string | null;
  departmentName?: string | null;
  positionName?: string | null;
  year?: number;
  month?: number;
  baseSalary?: number;
  overtimePay?: number;
  bonus?: number;
  grossIncome?: number;
  deductions?: number;
  laborInsurance?: number;
  healthInsurance?: number;
  totalDeductions?: number;
  netSalary?: number;
  workDays?: number;
  totalWorkHours?: number;
  totalOvertimeHours?: number;
  lateDays?: number;
  leaveDays?: number;
}
