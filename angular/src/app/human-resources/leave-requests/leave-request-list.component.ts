import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule, ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import {
  LeaveRequestService,
  EmployeeService,
  LeaveRequests,
  Employees,
  LeaveType,
  LeaveRequestStatus,
} from '@proxy/human-resources';

/**
 * 請假申請列表組件
 * 提供請假申請的建立、審核、查詢和狀態管理功能
 */
@Component({
  selector: 'app-leave-request-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, PageModule, ThemeSharedModule],
  templateUrl: './leave-request-list.component.html',
})
export class LeaveRequestListComponent implements OnInit {
  private leaveRequestService = inject(LeaveRequestService);
  private employeeService = inject(EmployeeService);
  private fb = inject(FormBuilder);
  private confirmationService = inject(ConfirmationService);

  /** 請假申請列表數據 */
  leaveRequests: LeaveRequests.LeaveRequestDto[] = [];
  /** 員工列表數據 */
  employees: Employees.EmployeeListDto[] = [];
  /** 新增請假申請模態框開關狀態 */
  isModalOpen = false;
  /** 請假詳情模態框開關狀態 */
  isDetailModalOpen = false;
  /** 當前選中的請假申請 */
  selectedRequest: LeaveRequests.LeaveRequestDto | null = null;
  /** 請假申請表單 */
  form!: FormGroup;
  /** 狀態篩選條件 */
  statusFilter: LeaveRequestStatus | null = null;
  /** 請假類型篩選條件 */
  leaveTypeFilter: LeaveType | null = null;

  /** 請假類型枚舉 */
  LeaveType = LeaveType;
  /** 請假申請狀態枚舉 */
  LeaveRequestStatus = LeaveRequestStatus;

  /**
   * 組件初始化
   * 載入請假申請、員工列表並建立表單
   */
  ngOnInit() {
    this.loadLeaveRequests();
    this.loadEmployees();
    this.buildForm();
  }

  /**
   * 載入請假申請列表
   * 根據篩選條件查詢請假申請
   */
  loadLeaveRequests() {
    const input: LeaveRequests.GetLeaveRequestsInput = { maxResultCount: 100 };
    if (this.statusFilter) input.status = this.statusFilter;
    if (this.leaveTypeFilter) input.leaveType = this.leaveTypeFilter;

    this.leaveRequestService.getList(input).subscribe(response => {
      this.leaveRequests = response.items || [];
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
   * 建立請假申請表單
   */
  buildForm() {
    this.form = this.fb.group({
      employeeId: [null, Validators.required],
      leaveType: [LeaveType.Annual, Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      hours: [8, [Validators.required, Validators.min(0.5)]],
      reason: ['', Validators.maxLength(500)],
    });
  }

  /**
   * 開啟新增請假申請模態框
   */
  openCreateModal() {
    this.form.reset({ leaveType: LeaveType.Annual, hours: 8 });
    this.isModalOpen = true;
  }

  /**
   * 關閉新增請假申請模態框
   */
  closeModal() {
    this.isModalOpen = false;
  }

  /**
   * 儲存請假申請
   * 建立新的請假申請記錄
   */
  save() {
    if (this.form.invalid) return;

    const data: LeaveRequests.CreateLeaveRequestDto = this.form.value;
    this.leaveRequestService.create(data).subscribe(() => {
      this.closeModal();
      this.loadLeaveRequests();
    });
  }

  /**
   * 查看請假申請詳情
   * @param request 請假申請記錄
   */
  viewDetail(request: LeaveRequests.LeaveRequestDto) {
    this.selectedRequest = request;
    this.isDetailModalOpen = true;
  }

  /**
   * 關閉請假詳情模態框
   */
  closeDetailModal() {
    this.isDetailModalOpen = false;
    this.selectedRequest = null;
  }

  /**
   * 核准請假申請
   * @param request 請假申請記錄
   */
  approve(request: LeaveRequests.LeaveRequestDto) {
    this.confirmationService
      .info('確定要核准此請假申請嗎？', '確認核准')
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.leaveRequestService.approve(request.id!, {}).subscribe(() => {
            this.loadLeaveRequests();
          });
        }
      });
  }

  /**
   * 駁回請假申請
   * @param request 請假申請記錄
   */
  reject(request: LeaveRequests.LeaveRequestDto) {
    this.confirmationService
      .warn('確定要駁回此請假申請嗎？', '確認駁回')
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.leaveRequestService.reject(request.id!, {}).subscribe(() => {
            this.loadLeaveRequests();
          });
        }
      });
  }

  /**
   * 獲取請假類型文字
   * @param type 請假類型
   * @returns 類型顯示文字
   */
  getLeaveTypeText(type: LeaveType): string {
    switch (type) {
      case LeaveType.Annual: return '特休';
      case LeaveType.Sick: return '病假';
      case LeaveType.Personal: return '事假';
      case LeaveType.Maternity: return '產假';
      case LeaveType.Paternity: return '陪產假';
      case LeaveType.Unpaid: return '無薪假';
      default: return '未知';
    }
  }

  /**
   * 獲取請假申請狀態文字
   * @param status 請假申請狀態
   * @returns 狀態顯示文字
   */
  getStatusText(status: LeaveRequestStatus): string {
    switch (status) {
      case LeaveRequestStatus.Pending: return '待審核';
      case LeaveRequestStatus.Approved: return '已核准';
      case LeaveRequestStatus.Rejected: return '已駁回';
      case LeaveRequestStatus.Cancelled: return '已取消';
      default: return '未知';
    }
  }

  /**
   * 獲取狀態徽章樣式類別
   * @param status 請假申請狀態
   * @returns CSS 類別名稱
   */
  getStatusBadgeClass(status: LeaveRequestStatus): string {
    switch (status) {
      case LeaveRequestStatus.Pending: return 'badge bg-warning';
      case LeaveRequestStatus.Approved: return 'badge bg-success';
      case LeaveRequestStatus.Rejected: return 'badge bg-danger';
      case LeaveRequestStatus.Cancelled: return 'badge bg-secondary';
      default: return 'badge bg-secondary';
    }
  }
}
