import { Routes } from '@angular/router';

export const humanResourcesRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'departments',
        loadComponent: () =>
          import('./departments/department-list.component').then(m => m.DepartmentListComponent),
      },
      {
        path: 'positions',
        loadComponent: () =>
          import('./positions/position-list.component').then(m => m.PositionListComponent),
      },
      {
        path: 'employees',
        loadComponent: () =>
          import('./employees/employee-list.component').then(m => m.EmployeeListComponent),
      },
      {
        path: 'leave-requests',
        loadComponent: () =>
          import('./leave-requests/leave-request-list.component').then(m => m.LeaveRequestListComponent),
      },
      {
        path: 'attendance',
        loadComponent: () =>
          import('./attendance/attendance-list.component').then(m => m.AttendanceListComponent),
      },
      {
        path: 'clock-in-out',
        loadComponent: () =>
          import('./attendance/clock-in-out.component').then(m => m.ClockInOutComponent),
      },
      {
        path: 'salaries',
        loadComponent: () =>
          import('./salaries/salary-list.component').then(m => m.SalaryListComponent),
      },
      {
        path: 'salary-calculation',
        loadComponent: () =>
          import('./salaries/salary-calculation.component').then(m => m.SalaryCalculationComponent),
      },
    ],
  },
];
