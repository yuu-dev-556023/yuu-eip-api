import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule, ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import {
  SalaryService,
  DepartmentService,
  Salaries,
  Departments,
  SalaryStatus,
} from '@proxy/human-resources';

/**
 * 薪資記錄列表組件
 * 提供薪資記錄的查詢、確認、發放和薪資單查看功能
 */
@Component({
  selector: 'app-salary-list',
  standalone: true,
  imports: [CommonModule, FormsModule, PageModule, ThemeSharedModule],
  templateUrl: './salary-list.component.html',
})
export class SalaryListComponent implements OnInit {
  private salaryService = inject(SalaryService);
  private departmentService = inject(DepartmentService);
  private confirmationService = inject(ConfirmationService);

  /** 薪資記錄列表數據 */
  salaryRecords: Salaries.SalaryRecordDto[] = [];
  /** 部門列表數據 */
  departments: Departments.DepartmentDto[] = [];
  /** 選中的薪資單 */
  selectedSlip: Salaries.SalarySlipDto | null = null;
  /** 薪資單模態框開關狀態 */
  isSlipModalOpen = false;

  /** 年份篩選 */
  yearFilter = new Date().getFullYear();
  /** 月份篩選 */
  monthFilter: number | null = null;
  /** 部門篩選 */
  departmentFilter: string | null = null;
  /** 狀態篩選 */
  statusFilter: SalaryStatus | null = null;

  /** 年份選項列表 */
  years: number[] = [];
  /** 月份選項列表 */
  months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

  /** 薪資狀態枚舉 */
  SalaryStatus = SalaryStatus;

  /**
   * 組件初始化
   * 初始化年份選項，載入部門和薪資記錄
   */
  ngOnInit() {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear - 2; i <= currentYear + 1; i++) {
      this.years.push(i);
    }

    this.loadDepartments();
    this.loadSalaries();
  }

  /**
   * 載入部門列表
   */
  loadDepartments() {
    this.departmentService.getAll().subscribe(response => {
      this.departments = response;
    });
  }

  /**
   * 載入薪資記錄
   * 根據篩選條件查詢薪資記錄
   */
  loadSalaries() {
    const input: Salaries.GetSalaryRecordsInput = { maxResultCount: 200 };
    if (this.yearFilter) input.year = this.yearFilter;
    if (this.monthFilter) input.month = this.monthFilter;
    if (this.departmentFilter) input.departmentId = this.departmentFilter;
    if (this.statusFilter) input.status = this.statusFilter;

    this.salaryService.getList(input).subscribe(response => {
      this.salaryRecords = response.items || [];
    });
  }

  /**
   * 確認薪資記錄
   * @param record 薪資記錄
   */
  confirm(record: Salaries.SalaryRecordDto) {
    this.confirmationService
      .info('確定要確認此薪資記錄嗎？', '確認')
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.salaryService.confirm(record.id!).subscribe(() => {
            this.loadSalaries();
          });
        }
      });
  }

  /**
   * 標記薪資為已發放
   * @param record 薪資記錄
   */
  markAsPaid(record: Salaries.SalaryRecordDto) {
    this.confirmationService
      .info('確定要標記此薪資為已發放嗎？', '確認')
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.salaryService.markAsPaid(record.id!).subscribe(() => {
            this.loadSalaries();
          });
        }
      });
  }

  /**
   * 查看薪資單詳情
   * @param record 薪資記錄
   */
  viewSlip(record: Salaries.SalaryRecordDto) {
    this.salaryService.getSalarySlip(record.id!).subscribe(response => {
      this.selectedSlip = response;
      this.isSlipModalOpen = true;
    });
  }

  /**
   * 關閉薪資單模態框
   */
  closeSlipModal() {
    this.isSlipModalOpen = false;
    this.selectedSlip = null;
  }

  /**
   * 獲取薪資狀態文字
   * @param status 薪資狀態
   * @returns 狀態顯示文字
   */
  getStatusText(status: SalaryStatus): string {
    switch (status) {
      case SalaryStatus.Pending: return '待核算';
      case SalaryStatus.Confirmed: return '已確認';
      case SalaryStatus.Paid: return '已發放';
      default: return '未知';
    }
  }

  /**
   * 獲取狀態徽章樣式類別
   * @param status 薪資狀態
   * @returns CSS 類別名稱
   */
  getStatusBadgeClass(status: SalaryStatus): string {
    switch (status) {
      case SalaryStatus.Pending: return 'badge bg-warning';
      case SalaryStatus.Confirmed: return 'badge bg-primary';
      case SalaryStatus.Paid: return 'badge bg-success';
      default: return 'badge bg-secondary';
    }
  }
}
