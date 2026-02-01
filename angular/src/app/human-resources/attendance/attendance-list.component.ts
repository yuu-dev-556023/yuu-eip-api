import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  AttendanceService,
  DepartmentService,
  Attendance,
  Departments,
  AttendanceStatus,
} from '@proxy/human-resources';

/**
 * 考勤記錄列表組件
 * 提供員工考勤記錄的查詢、篩選和狀態顯示功能
 */
@Component({
  selector: 'app-attendance-list',
  standalone: true,
  imports: [CommonModule, FormsModule, PageModule, ThemeSharedModule],
  templateUrl: './attendance-list.component.html',
})
export class AttendanceListComponent implements OnInit {
  private attendanceService = inject(AttendanceService);
  private departmentService = inject(DepartmentService);

  /** 考勤記錄列表數據 */
  attendanceRecords: Attendance.AttendanceRecordDto[] = [];
  /** 部門列表數據 */
  departments: Departments.DepartmentDto[] = [];
  /** 部門篩選條件 */
  departmentFilter: string | null = null;
  /** 開始日期篩選 */
  dateFrom: string | null = null;
  /** 結束日期篩選 */
  dateTo: string | null = null;
  /** 狀態篩選條件 */
  statusFilter: AttendanceStatus | null = null;

  /** 考勤狀態枚舉 */
  AttendanceStatus = AttendanceStatus;

  /**
   * 組件初始化
   * 設定預設日期範圍為當月，載入部門和考勤記錄
   */
  ngOnInit() {
    // Set default date range to current month
    const now = new Date();
    this.dateFrom = new Date(now.getFullYear(), now.getMonth(), 1).toISOString().split('T')[0];
    this.dateTo = new Date(now.getFullYear(), now.getMonth() + 1, 0).toISOString().split('T')[0];

    this.loadDepartments();
    this.loadAttendance();
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
   * 載入考勤記錄
   * 根據篩選條件查詢考勤記錄
   */
  loadAttendance() {
    const input: Attendance.GetAttendanceInput = { maxResultCount: 100 };
    if (this.departmentFilter) input.departmentId = this.departmentFilter;
    if (this.dateFrom) input.dateFrom = this.dateFrom;
    if (this.dateTo) input.dateTo = this.dateTo;
    if (this.statusFilter) input.status = this.statusFilter;

    this.attendanceService.getList(input).subscribe(response => {
      this.attendanceRecords = response.items || [];
    });
  }

  /**
   * 獲取考勤狀態文字
   * @param status 考勤狀態
   * @returns 狀態顯示文字
   */
  getStatusText(status: AttendanceStatus): string {
    switch (status) {
      case AttendanceStatus.Normal: return '正常';
      case AttendanceStatus.Late: return '遲到';
      case AttendanceStatus.EarlyLeave: return '早退';
      case AttendanceStatus.Absent: return '曠職';
      case AttendanceStatus.OnLeave: return '請假';
      default: return '未知';
    }
  }

  /**
   * 獲取狀態徽章樣式類別
   * @param status 考勤狀態
   * @returns CSS 類別名稱
   */
  getStatusBadgeClass(status: AttendanceStatus): string {
    switch (status) {
      case AttendanceStatus.Normal: return 'badge bg-success';
      case AttendanceStatus.Late: return 'badge bg-warning';
      case AttendanceStatus.EarlyLeave: return 'badge bg-warning';
      case AttendanceStatus.Absent: return 'badge bg-danger';
      case AttendanceStatus.OnLeave: return 'badge bg-info';
      default: return 'badge bg-secondary';
    }
  }
}
