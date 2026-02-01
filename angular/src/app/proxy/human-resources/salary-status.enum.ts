import { mapEnumToOptions } from '@abp/ng.core';

export enum SalaryStatus {
  Pending = 1,
  Confirmed = 2,
  Paid = 3,
}

export const salaryStatusOptions = mapEnumToOptions(SalaryStatus);
