import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule, ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import {
  DepartmentService,
  Departments,
} from '@proxy/human-resources';

/**
 * 部門列表組件
 * 提供部門的增刪改查功能
 */
@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, PageModule, ThemeSharedModule],
  templateUrl: './department-list.component.html',
})
export class DepartmentListComponent implements OnInit {
  private departmentService = inject(DepartmentService);
  private fb = inject(FormBuilder);
  private confirmationService = inject(ConfirmationService);

  /** 部門列表數據 */
  departments: Departments.DepartmentDto[] = [];
  /** 所有部門數據（用於下拉選擇） */
  allDepartments: Departments.DepartmentDto[] = [];
  /** 模態框開關狀態 */
  isModalOpen = false;
  /** 當前選中的部門 */
  selectedDepartment: Departments.DepartmentDto | null = null;
  /** 部門表單 */
  form!: FormGroup;

  ngOnInit() {
    this.loadDepartments();
    this.buildForm();
  }

  /**
   * 加載部門數據
   */
  loadDepartments() {
    this.departmentService.getList({ maxResultCount: 100 } as Departments.GetDepartmentsInput).subscribe(response => {
      this.departments = response.items || [];
    });

    this.departmentService.getAll().subscribe(response => {
      this.allDepartments = response;
    });
  }

  /**
   * 建立部門表單
   */
  buildForm() {
    this.form = this.fb.group({
      code: ['', [Validators.required, Validators.maxLength(32)]],
      name: ['', [Validators.required, Validators.maxLength(128)]],
      parentId: [null],
      managerId: [null],
      order: [0],
      isActive: [true],
    });
  }

  /**
   * 開啟創建部門模態框
   */
  openCreateModal() {
    this.selectedDepartment = null;
    this.form.reset({ order: 0, isActive: true });
    this.isModalOpen = true;
  }

  /**
   * 開啟編輯部門模態框
   * @param department 要編輯的部門對象
   */
  openEditModal(department: Departments.DepartmentDto) {
    this.selectedDepartment = department;
    this.form.patchValue({
      code: department.code,
      name: department.name,
      parentId: department.parentId,
      managerId: department.managerId,
      order: department.order,
      isActive: department.isActive,
    });
    this.isModalOpen = true;
  }

  /**
   * 關閉模態框
   */
  closeModal() {
    this.isModalOpen = false;
    this.selectedDepartment = null;
  }

  /**
   * 保存部門數據（新增或更新）
   */
  save() {
    if (this.form.invalid) return;

    const data: Departments.CreateUpdateDepartmentDto = this.form.value;

    if (this.selectedDepartment) {
      this.departmentService.update(this.selectedDepartment.id!, data).subscribe(() => {
        this.closeModal();
        this.loadDepartments();
      });
    } else {
      this.departmentService.create(data).subscribe(() => {
        this.closeModal();
        this.loadDepartments();
      });
    }
  }

  /**
   * 刪除部門
   * @param department 要刪除的部門對象
   */
  delete(department: Departments.DepartmentDto) {
    this.confirmationService
      .warn('確定要刪除此部門嗎？', '確認刪除')
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.departmentService.delete(department.id!).subscribe(() => {
            this.loadDepartments();
          });
        }
      });
  }
}
