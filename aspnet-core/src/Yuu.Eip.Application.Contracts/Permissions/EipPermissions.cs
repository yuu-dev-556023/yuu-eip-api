namespace Yuu.Eip.Permissions;

public static class EipPermissions
{
    public const string GroupName = "Eip";

    /* Add your own permission names. Example: */
    /* public const string MyPermission1 = GroupName + ".MyPermission1"; */

    public static class Todo
    {
        public const string Default = GroupName + ".Todo";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class HumanResources
    {
        public const string GroupName = "HumanResources";

        public static class Departments
        {
            public const string Default = GroupName + ".Departments";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class Positions
        {
            public const string Default = GroupName + ".Positions";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class Employees
        {
            public const string Default = GroupName + ".Employees";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class LeaveRequests
        {
            public const string Default = GroupName + ".LeaveRequests";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string Approve = Default + ".Approve";
        }

        public static class Attendance
        {
            public const string Default = GroupName + ".Attendance";
            public const string ClockInOut = Default + ".ClockInOut";
            public const string ManageRecords = Default + ".ManageRecords";
            public const string ViewReports = Default + ".ViewReports";
        }

        public static class Salaries
        {
            public const string Default = GroupName + ".Salaries";
            public const string Calculate = Default + ".Calculate";
            public const string Confirm = Default + ".Confirm";
            public const string ViewAll = Default + ".ViewAll";
        }
    }
}
