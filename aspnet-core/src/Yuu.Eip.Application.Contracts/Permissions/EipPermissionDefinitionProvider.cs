using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Yuu.Eip.Localization;

namespace Yuu.Eip.Permissions;

public class EipPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EipPermissions.GroupName);
        /* Define your own permissions here. Example: */
        /* myGroup.AddPermission(EipPermissions.MyPermission1, L("Permission:MyPermission1")); */

        /* Todo permissions */
        /* var todoPermission = myGroup.AddPermission(EipPermissions.Todo.Default, L("Permission:Todo")); */
        /* todoPermission.AddChild(EipPermissions.Todo.Create, L("Permission:Todo.Create")); */
        /* todoPermission.AddChild(EipPermissions.Todo.Update, L("Permission:Todo.Update")); */
        /* todoPermission.AddChild(EipPermissions.Todo.Delete, L("Permission:Todo.Delete")); */

        DefineHumanResourcesPermissions(context);
    }

    private void DefineHumanResourcesPermissions(IPermissionDefinitionContext context)
    {
        var hrGroup = context.AddGroup(
            EipPermissions.HumanResources.GroupName,
            L("Permission:HumanResources"));

        /* Departments */
        var departmentsPermission = hrGroup.AddPermission(
            EipPermissions.HumanResources.Departments.Default,
            L("Permission:HumanResources.Departments"));
        departmentsPermission.AddChild(
            EipPermissions.HumanResources.Departments.Create,
            L("Permission:HumanResources.Departments.Create"));
        departmentsPermission.AddChild(
            EipPermissions.HumanResources.Departments.Update,
            L("Permission:HumanResources.Departments.Update"));
        departmentsPermission.AddChild(
            EipPermissions.HumanResources.Departments.Delete,
            L("Permission:HumanResources.Departments.Delete"));

        /* Positions */
        var positionsPermission = hrGroup.AddPermission(
            EipPermissions.HumanResources.Positions.Default,
            L("Permission:HumanResources.Positions"));
        positionsPermission.AddChild(
            EipPermissions.HumanResources.Positions.Create,
            L("Permission:HumanResources.Positions.Create"));
        positionsPermission.AddChild(
            EipPermissions.HumanResources.Positions.Update,
            L("Permission:HumanResources.Positions.Update"));
        positionsPermission.AddChild(
            EipPermissions.HumanResources.Positions.Delete,
            L("Permission:HumanResources.Positions.Delete"));

        /* Employees */
        var employeesPermission = hrGroup.AddPermission(
            EipPermissions.HumanResources.Employees.Default,
            L("Permission:HumanResources.Employees"));
        employeesPermission.AddChild(
            EipPermissions.HumanResources.Employees.Create,
            L("Permission:HumanResources.Employees.Create"));
        employeesPermission.AddChild(
            EipPermissions.HumanResources.Employees.Update,
            L("Permission:HumanResources.Employees.Update"));
        employeesPermission.AddChild(
            EipPermissions.HumanResources.Employees.Delete,
            L("Permission:HumanResources.Employees.Delete"));

        /* LeaveRequests */
        var leaveRequestsPermission = hrGroup.AddPermission(
            EipPermissions.HumanResources.LeaveRequests.Default,
            L("Permission:HumanResources.LeaveRequests"));
        leaveRequestsPermission.AddChild(
            EipPermissions.HumanResources.LeaveRequests.Create,
            L("Permission:HumanResources.LeaveRequests.Create"));
        leaveRequestsPermission.AddChild(
            EipPermissions.HumanResources.LeaveRequests.Update,
            L("Permission:HumanResources.LeaveRequests.Update"));
        leaveRequestsPermission.AddChild(
            EipPermissions.HumanResources.LeaveRequests.Delete,
            L("Permission:HumanResources.LeaveRequests.Delete"));
        leaveRequestsPermission.AddChild(
            EipPermissions.HumanResources.LeaveRequests.Approve,
            L("Permission:HumanResources.LeaveRequests.Approve"));

        /* Attendance */
        var attendancePermission = hrGroup.AddPermission(
            EipPermissions.HumanResources.Attendance.Default,
            L("Permission:HumanResources.Attendance"));
        attendancePermission.AddChild(
            EipPermissions.HumanResources.Attendance.ClockInOut,
            L("Permission:HumanResources.Attendance.ClockInOut"));
        attendancePermission.AddChild(
            EipPermissions.HumanResources.Attendance.ManageRecords,
            L("Permission:HumanResources.Attendance.ManageRecords"));
        attendancePermission.AddChild(
            EipPermissions.HumanResources.Attendance.ViewReports,
            L("Permission:HumanResources.Attendance.ViewReports"));

        /* Salaries */
        var salariesPermission = hrGroup.AddPermission(
            EipPermissions.HumanResources.Salaries.Default,
            L("Permission:HumanResources.Salaries"));
        salariesPermission.AddChild(
            EipPermissions.HumanResources.Salaries.Calculate,
            L("Permission:HumanResources.Salaries.Calculate"));
        salariesPermission.AddChild(
            EipPermissions.HumanResources.Salaries.Confirm,
            L("Permission:HumanResources.Salaries.Confirm"));
        salariesPermission.AddChild(
            EipPermissions.HumanResources.Salaries.ViewAll,
            L("Permission:HumanResources.Salaries.ViewAll"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EipResource>(name);
    }
}
