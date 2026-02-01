import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PageModule } from '@abp/ng.components/page';
import { ThemeSharedModule, ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import {
  EmployeeService,
  DepartmentService,
  PositionService,
  Employees,
  Departments,
  Positions,
  Gender,
  EmployeeStatus,
} from '@proxy/human-resources';

/**
 * 員工列表組件
 * 提供員工的增刪改查功能，包含篩選和狀態管理
 */
@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, PageModule, ThemeSharedModule],
  templateUrl: './employee-list.component.html',
})
export class EmployeeListComponent implements OnInit {
  private employeeService = inject(EmployeeService);
  private departmentService = inject(DepartmentService);
  private positionService = inject(PositionService);
  private fb = inject(FormBuilder);
  private confirmationService = inject(ConfirmationService);

  /** 員工列表數據 */
  employees: Employees.EmployeeListDto[] = [];
  /** 部門列表數據 */
  departments: Departments.DepartmentDto[] = [];
  /** 職位列表數據 */
  positions: Positions.PositionDto[] = [];
  /** 模態框開關狀態 */
  isModalOpen = false;
  /** 當前選中的員工 */
  selectedEmployee: Employees.EmployeeDto | null = null;
  /** 員工表單 */
  form!: FormGroup;
  /** 篩選關鍵字 */
  filter = '';
  /** 狀態篩選 */
  statusFilter: EmployeeStatus | null = null;

  /** 性別枚舉 */
  Gender = Gender;
  /** 員工狀態枚舉 */
  EmployeeStatus = EmployeeStatus;

  ngOnInit() {
    this.loadEmployees();
    this.loadDepartments();
    this.loadPositions();
    this.buildForm();
  }

  loadEmployees() {
    const input: Employees.GetEmployeesInput = { maxResultCount: 100 };
    if (this.filter) input.filter = this.filter;
    if (this.statusFilter) input.status = this.statusFilter;

    this.employeeService.getList(input).subscribe(response => {
      this.employees = response.items || [];
    });
  }

  loadDepartments() {
    this.departmentService.getAll().subscribe(response => {
      this.departments = response;
    });
  }

  loadPositions() {
    this.positionService.getList({ maxResultCount: 100 } as Positions.GetPositionsInput).subscribe(response => {
      this.positions = response.items || [];
    });
  }

  buildForm() {
    this.form = this.fb.group({
      employeeNo: ['', [Validators.required, Validators.maxLength(32)]],
      name: ['', [Validators.required, Validators.maxLength(64)]],
      email: ['', [Validators.email, Validators.maxLength(256)]],
      phone: ['', Validators.maxLength(32)],
      birthDate: [null],
      gender: [Gender.Male, Validators.required],
      hireDate: ['', Validators.required],
      departmentId: [null, Validators.required],
      positionId: [null, Validators.required],
      managerId: [null],
      status: [EmployeeStatus.Active],
      terminationDate: [null],
    });
  }

  openCreateModal() {
    this.selectedEmployee = null;
    this.form.reset({ gender: Gender.Male, status: EmployeeStatus.Active });
    this.isModalOpen = true;
  }

  openEditModal(employee: Employees.EmployeeListDto) {
    this.employeeService.get(employee.id!).subscribe(emp => {
      this.selectedEmployee = emp;
      this.form.patchValue({
        employeeNo: emp.employeeNo,
        name: emp.name,
        email: emp.email,
        phone: emp.phone,
        birthDate: emp.birthDate?.split('T')[0],
        gender: emp.gender,
        hireDate: emp.hireDate?.split('T')[0],
        departmentId: emp.departmentId,
        positionId: emp.positionId,
        managerId: emp.managerId,
        status: emp.status,
        terminationDate: emp.terminationDate?.split('T')[0],
      });
      this.isModalOpen = true;
    });
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedEmployee = null;
  }

  save() {
    if (this.form.invalid) return;

    const formValue = this.form.value;

    if (this.selectedEmployee) {
      const data: Employees.UpdateEmployeeDto = {
        name: formValue.name,
        email: formValue.email,
        phone: formValue.phone,
        birthDate: formValue.birthDate,
        gender: formValue.gender,
        terminationDate: formValue.terminationDate,
        status: formValue.status,
        departmentId: formValue.departmentId,
        positionId: formValue.positionId,
        managerId: formValue.managerId,
      };
      this.employeeService.update(this.selectedEmployee.id!, data).subscribe(() => {
        this.closeModal();
        this.loadEmployees();
      });
    } else {
      const data: Employees.CreateEmployeeDto = {
        employeeNo: formValue.employeeNo,
        name: formValue.name,
        email: formValue.email,
        phone: formValue.phone,
        birthDate: formValue.birthDate,
        gender: formValue.gender,
        hireDate: formValue.hireDate,
        departmentId: formValue.departmentId,
        positionId: formValue.positionId,
        managerId: formValue.managerId,
      };
      this.employeeService.create(data).subscribe(() => {
        this.closeModal();
        this.loadEmployees();
      });
    }
  }

  delete(employee: Employees.EmployeeListDto) {
    this.confirmationService
      .warn('確定要刪除此員工嗎？', '確認刪除')
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.employeeService.delete(employee.id!).subscribe(() => {
            this.loadEmployees();
          });
        }
      });
  }

  getStatusText(status: EmployeeStatus): string {
    switch (status) {
      case EmployeeStatus.Active: return '在職';
      case EmployeeStatus.Resigned: return '離職';
      case EmployeeStatus.OnLeave: return '留停';
      default: return '未知';
    }
  }

  getStatusBadgeClass(status: EmployeeStatus): string {
    switch (status) {
      case EmployeeStatus.Active: return 'badge bg-success';
      case EmployeeStatus.Resigned: return 'badge bg-danger';
      case EmployeeStatus.OnLeave: return 'badge bg-warning';
      default: return 'badge bg-secondary';
    }
  }
}
