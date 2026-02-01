import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule, ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import {
  PositionService,
  Positions,
} from '@proxy/human-resources';

/**
 * 職位列表組件
 * 提供職位的增刪改查功能，包含職位等級和薪資設定
 */
@Component({
  selector: 'app-position-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, PageModule, ThemeSharedModule],
  templateUrl: './position-list.component.html',
})
export class PositionListComponent implements OnInit {
  private positionService = inject(PositionService);
  private fb = inject(FormBuilder);
  private confirmationService = inject(ConfirmationService);

  /** 職位列表數據 */
  positions: Positions.PositionDto[] = [];
  /** 模態框開關狀態 */
  isModalOpen = false;
  /** 當前選中的職位 */
  selectedPosition: Positions.PositionDto | null = null;
  /** 職位表單 */
  form!: FormGroup;

  /**
   * 組件初始化
   * 載入職位列表並建立表單
   */
  ngOnInit() {
    this.loadPositions();
    this.buildForm();
  }

  /**
   * 載入職位列表
   */
  loadPositions() {
    this.positionService.getList({ maxResultCount: 100 } as Positions.GetPositionsInput).subscribe(response => {
      this.positions = response.items || [];
    });
  }

  /**
   * 建立職位表單
   */
  buildForm() {
    this.form = this.fb.group({
      code: ['', [Validators.required, Validators.maxLength(32)]],
      name: ['', [Validators.required, Validators.maxLength(128)]],
      level: [1, [Validators.required, Validators.min(1), Validators.max(100)]],
      baseSalary: [0, [Validators.required, Validators.min(0)]],
      isActive: [true],
    });
  }

  /**
   * 開啟新增職位模態框
   */
  openCreateModal() {
    this.selectedPosition = null;
    this.form.reset({ level: 1, baseSalary: 0, isActive: true });
    this.isModalOpen = true;
  }

  /**
   * 開啟編輯職位模態框
   * @param position 職位記錄
   */
  openEditModal(position: Positions.PositionDto) {
    this.selectedPosition = position;
    this.form.patchValue({
      code: position.code,
      name: position.name,
      level: position.level,
      baseSalary: position.baseSalary,
      isActive: position.isActive,
    });
    this.isModalOpen = true;
  }

  /**
   * 關閉模態框
   */
  closeModal() {
    this.isModalOpen = false;
    this.selectedPosition = null;
  }

  /**
   * 儲存職位資訊
   * 新增或更新職位記錄
   */
  save() {
    if (this.form.invalid) return;

    const data: Positions.CreateUpdatePositionDto = this.form.value;

    if (this.selectedPosition) {
      this.positionService.update(this.selectedPosition.id!, data).subscribe(() => {
        this.closeModal();
        this.loadPositions();
      });
    } else {
      this.positionService.create(data).subscribe(() => {
        this.closeModal();
        this.loadPositions();
      });
    }
  }

  /**
   * 刪除職位
   * @param position 職位記錄
   */
  delete(position: Positions.PositionDto) {
    this.confirmationService
      .warn('確定要刪除此職位嗎？', '確認刪除')
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.positionService.delete(position.id!).subscribe(() => {
            this.loadPositions();
          });
        }
      });
  }
}
