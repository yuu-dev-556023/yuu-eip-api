using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Yuu.Eip.HumanResources;

namespace Yuu.Eip.EntityFrameworkCore;

public static class HumanResourcesModelCreatingExtensions
{
    public static void ConfigureHumanResources(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Department>(b =>
        {
            b.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(HumanResourcesConsts.MaxDepartmentCodeLength);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(HumanResourcesConsts.MaxDepartmentNameLength);

            b.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
            b.HasIndex(x => x.ParentId);
            b.HasIndex(x => x.ManagerId);
        });

        builder.Entity<Position>(b =>
        {
            b.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(HumanResourcesConsts.MaxPositionCodeLength);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(HumanResourcesConsts.MaxPositionNameLength);

            b.Property(x => x.BaseSalary)
                .HasPrecision(18, 2);

            b.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        });

        builder.Entity<Employee>(b =>
        {
            b.Property(x => x.EmployeeNo)
                .IsRequired()
                .HasMaxLength(HumanResourcesConsts.MaxEmployeeNoLength);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(HumanResourcesConsts.MaxEmployeeNameLength);

            b.Property(x => x.Email)
                .HasMaxLength(HumanResourcesConsts.MaxEmailLength);

            b.Property(x => x.Phone)
                .HasMaxLength(HumanResourcesConsts.MaxPhoneLength);

            b.HasIndex(x => new { x.TenantId, x.EmployeeNo }).IsUnique();
            b.HasIndex(x => x.DepartmentId);
            b.HasIndex(x => x.PositionId);
            b.HasIndex(x => x.ManagerId);
            b.HasIndex(x => x.UserId);
            b.HasIndex(x => x.Status);
        });

        builder.Entity<LeaveRequest>(b =>
        {
            b.Property(x => x.Hours)
                .HasPrecision(8, 2);

            b.Property(x => x.Reason)
                .HasMaxLength(HumanResourcesConsts.MaxLeaveReasonLength);

            b.Property(x => x.ApproverComment)
                .HasMaxLength(HumanResourcesConsts.MaxApproverCommentLength);

            b.HasIndex(x => x.EmployeeId);
            b.HasIndex(x => x.ApproverId);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => new { x.StartDate, x.EndDate });
        });

        builder.Entity<AttendanceRecord>(b =>
        {
            b.Property(x => x.WorkHours)
                .HasPrecision(8, 2);

            b.Property(x => x.OvertimeHours)
                .HasPrecision(8, 2);

            b.Property(x => x.Note)
                .HasMaxLength(HumanResourcesConsts.MaxNoteLength);

            b.HasIndex(x => x.EmployeeId);
            b.HasIndex(x => x.Date);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => new { x.EmployeeId, x.Date }).IsUnique();
        });

        builder.Entity<SalaryRecord>(b =>
        {
            b.Property(x => x.BaseSalary)
                .HasPrecision(18, 2);

            b.Property(x => x.OvertimePay)
                .HasPrecision(18, 2);

            b.Property(x => x.Bonus)
                .HasPrecision(18, 2);

            b.Property(x => x.Deductions)
                .HasPrecision(18, 2);

            b.Property(x => x.LaborInsurance)
                .HasPrecision(18, 2);

            b.Property(x => x.HealthInsurance)
                .HasPrecision(18, 2);

            b.Property(x => x.NetSalary)
                .HasPrecision(18, 2);

            b.Property(x => x.Note)
                .HasMaxLength(HumanResourcesConsts.MaxNoteLength);

            b.HasIndex(x => x.EmployeeId);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => new { x.EmployeeId, x.Year, x.Month }).IsUnique();
        });
    }
}
