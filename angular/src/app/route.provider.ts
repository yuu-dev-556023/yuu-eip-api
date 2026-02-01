import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureRoutes,
    deps: [RoutesService],
    multi: true,
  },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/human-resources',
        name: '::Menu:HumanResources',
        iconClass: 'fas fa-users',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy:
          'HumanResources.Departments || ' +
          'HumanResources.Positions || ' +
          'HumanResources.Employees || ' +
          'HumanResources.LeaveRequests || ' +
          'HumanResources.Attendance.ClockInOut || ' +
          'HumanResources.Attendance.ManageRecords || ' +
          'HumanResources.Salaries',
      },
      {
        path: '/human-resources/departments',
        name: '::Menu:HumanResources:Departments',
        parentName: '::Menu:HumanResources',
        iconClass: 'fas fa-sitemap',
        order: 1,
        layout: eLayoutType.application,
        requiredPolicy: 'HumanResources.Departments',
      },
      {
        path: '/human-resources/positions',
        name: '::Menu:HumanResources:Positions',
        parentName: '::Menu:HumanResources',
        iconClass: 'fas fa-id-badge',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: 'HumanResources.Positions',
      },
      {
        path: '/human-resources/employees',
        name: '::Menu:HumanResources:Employees',
        parentName: '::Menu:HumanResources',
        iconClass: 'fas fa-user-tie',
        order: 3,
        layout: eLayoutType.application,
        requiredPolicy: 'HumanResources.Employees',
      },
      {
        path: '/human-resources/leave-requests',
        name: '::Menu:HumanResources:LeaveRequests',
        parentName: '::Menu:HumanResources',
        iconClass: 'fas fa-calendar-alt',
        order: 4,
        layout: eLayoutType.application,
        requiredPolicy: 'HumanResources.LeaveRequests',
      },
      {
        path: '/human-resources/clock-in-out',
        name: '::Menu:HumanResources:ClockInOut',
        parentName: '::Menu:HumanResources',
        iconClass: 'fas fa-clock',
        order: 5,
        layout: eLayoutType.application,
        requiredPolicy: 'HumanResources.Attendance.ClockInOut',
      },
      {
        path: '/human-resources/attendance',
        name: '::Menu:HumanResources:Attendance',
        parentName: '::Menu:HumanResources',
        iconClass: 'fas fa-clipboard-list',
        order: 6,
        layout: eLayoutType.application,
        requiredPolicy: 'HumanResources.Attendance.ManageRecords',
      },
      {
        path: '/human-resources/salaries',
        name: '::Menu:HumanResources:Salaries',
        parentName: '::Menu:HumanResources',
        iconClass: 'fas fa-coins',
        order: 7,
        layout: eLayoutType.application,
        requiredPolicy: 'HumanResources.Salaries',
      },
    ]);
  };
}
