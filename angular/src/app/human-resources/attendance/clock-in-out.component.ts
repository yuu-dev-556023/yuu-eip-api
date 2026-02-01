import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  AttendanceService,
  Attendance,
  AttendanceStatus,
} from '@proxy/human-resources';

/**
 * 打卡簽到簽退組件
 * 提供員工每日打卡上下班功能，包含即時時間顯示和考勤記錄查詢
 */
@Component({
  selector: 'app-clock-in-out',
  standalone: true,
  imports: [CommonModule, FormsModule, PageModule, ThemeSharedModule],
  templateUrl: './clock-in-out.component.html',
})
export class ClockInOutComponent implements OnInit {
  private attendanceService = inject(AttendanceService);

  /** 當日考勤記錄 */
  todayRecord: Attendance.AttendanceRecordDto | null = null;
  /** 當前時間 */
  currentTime = new Date();
  /** 打卡備註 */
  note = '';
  /** 載入狀態 */
  loading = false;

  /** 考勤狀態枚舉 */
  AttendanceStatus = AttendanceStatus;

  /**
   * 組件初始化
   * 載入當日考勤記錄並啟動即時時間更新
   */
  ngOnInit() {
    this.loadTodayRecord();

    // Update current time every second
    setInterval(() => {
      this.currentTime = new Date();
    }, 1000);
  }

  /**
   * 載入當日考勤記錄
   */
  loadTodayRecord() {
    this.attendanceService.getTodayRecord().subscribe({
      next: response => {
        this.todayRecord = response;
      },
      error: () => {
        this.todayRecord = null;
      },
    });
  }

  /**
   * 簽到打卡
   * 記錄上班時間和備註
   */
  clockIn() {
    this.loading = true;
    const input: Attendance.ClockInOutDto = { note: this.note || null };
    this.attendanceService.clockIn(input).subscribe({
      next: response => {
        this.todayRecord = response;
        this.note = '';
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
    });
  }

  /**
   * 簽退打卡
   * 記錄下班時間和備註
   */
  clockOut() {
    this.loading = true;
    const input: Attendance.ClockInOutDto = { note: this.note || null };
    this.attendanceService.clockOut(input).subscribe({
      next: response => {
        this.todayRecord = response;
        this.note = '';
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
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
