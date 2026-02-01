import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule, ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import {
  SalaryService,
  DepartmentService,
  EmployeeService,
  Salaries,
  Departments,
  Employees,
} from '@proxy/human-resources';

/**
 * 薪資核算組件
 * 提供單一員工薪資核算和批次薪資核算功能
 */
@Component({
  selector: 'app-salary-calculation',
  standalone: true,
  imports: [CommonModule, FormsModule, PageModule, ThemeSharedModule],
  templateUrl: './salary-calculation.component.html',
})
export class SalaryCalculationComponent implements OnInit {
  private salaryService = inject(SalaryService);
  private departmentService = inject(DepartmentService);
  private employeeService = inject(EmployeeService);
  private confirmationService = inject(ConfirmationService);

  /** 部門列表數據 */
  departments: Departments.DepartmentDto[] = [];
  /** 員工列表數據 */
  employees: Employees.EmployeeListDto[] = [];
  /** 核算結果列表 */
  calculationResults: Salaries.SalaryRecordDto[] = [];

  // 單一核算相關屬性
  /** 選中的員工 ID */
  selectedEmployeeId: string | null = null;
  /** 單一核算年份 */
  singleYear = new Date().getFullYear();
  /** 單一核算月份 */
  singleMonth = new Date().getMonth() + 1;
  /** 單一核算獎金 */
  singleBonus = 0;
  /** 單一核算載入狀態 */
  calculating = false;

  // 批次核算相關屬性
  /** 批次核算部門 ID */
  batchDepartmentId: string | null = null;
  /** 批次核算年份 */
  batchYear = new Date().getFullYear();
  /** 批次核算月份 */
  batchMonth = new Date().getMonth() + 1;
  /** 批次核算載入狀態 */
  batchCalculating = false;

  /** 年份選項列表 */
  years: number[] = [];
  /** 月份選項列表 */
  months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];

  /**
   * 組件初始化
   * 初始化年份選項，載入部門和員工列表
   */
  ngOnInit() {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear - 2; i <= currentYear + 1; i++) {
      this.years.push(i);
    }

    this.loadDepartments();
    this.loadEmployees();
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
   * 載入員工列表
   */
  loadEmployees() {
    this.employeeService.getAll().subscribe(response => {
      this.employees = response;
    });
  }

  /**
   * 單一員工薪資核算
   * 根據選定員工、年月和獎金進行薪資核算
   */
  calculateSingle() {
    if (!this.selectedEmployeeId) return;

    this.calculating = true;
    const input: Salaries.CalculateSalaryInput = {
      employeeId: this.selectedEmployeeId,
      year: this.singleYear,
      month: this.singleMonth,
      bonus: this.singleBonus,
    };

    this.salaryService.calculate(input).subscribe({
      next: response => {
        this.calculationResults = [response];
        this.calculating = false;
      },
      error: () => {
        this.calculating = false;
      },
    });
  }

  /**
   * 批次薪資核算
   * 根據指定年月和部門進行批次薪資核算
   */
  calculateBatch() {
    this.confirmationService
      .info(
        `確定要批次核算 ${this.batchYear}年${this.batchMonth}月 的薪資嗎？`,
        '確認批次核算'
      )
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.batchCalculating = true;

          this.salaryService
            .batchCalculate(this.batchYear, this.batchMonth, this.batchDepartmentId ?? undefined)
            .subscribe({
              next: response => {
                this.calculationResults = response;
                this.batchCalculating = false;
              },
              error: () => {
                this.batchCalculating = false;
              },
            });
        }
      });
  }
}
