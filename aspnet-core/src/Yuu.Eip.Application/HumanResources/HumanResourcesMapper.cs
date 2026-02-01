using Riok.Mapperly.Abstractions;
using Yuu.Eip.HumanResources.Attendance;
using Yuu.Eip.HumanResources.Departments;
using Yuu.Eip.HumanResources.Employees;
using Yuu.Eip.HumanResources.LeaveRequests;
using Yuu.Eip.HumanResources.Positions;
using Yuu.Eip.HumanResources.Salaries;

namespace Yuu.Eip.HumanResources;

[Mapper]
public partial class HumanResourcesMapper
{
    /* Department */
    public partial DepartmentDto DepartmentToDto(Department department);

    /* Position */
    public partial PositionDto PositionToDto(Position position);

    /* Employee */
    public partial EmployeeDto EmployeeToDto(Employee employee);
    public partial EmployeeListDto EmployeeToListDto(Employee employee);

    /* LeaveRequest */
    public partial LeaveRequestDto LeaveRequestToDto(LeaveRequest leaveRequest);

    /* AttendanceRecord */
    public partial AttendanceRecordDto AttendanceRecordToDto(AttendanceRecord record);

    /* SalaryRecord */
    public partial SalaryRecordDto SalaryRecordToDto(SalaryRecord record);
}
