using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yuu.Eip.Migrations
{
    /// <inheritdoc />
    public partial class AddHumanResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attendance_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶Id"),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "員工Id"),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "打卡日期"),
                    clock_in = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "上班打卡時間"),
                    clock_out = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "下班打卡時間"),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "出勤狀態"),
                    work_hours = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false, comment: "工作時數"),
                    overtime_hours = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false, comment: "加班時數"),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "備註"),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記"),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "建立者 ID"),
                    last_modification_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "最後修改時間"),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "最後修改者 ID"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "是否已刪除"),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "刪除者 ID"),
                    deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "刪除時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attendance_records", x => x.id);
                    table.CheckConstraint("CK_attendance_records_status_Enum", "status BETWEEN 1 AND 5");
                },
                comment: "打卡記錄");

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶Id"),
                    code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "部門代碼"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "部門名稱"),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "上級部門Id"),
                    manager_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "部門主管Id"),
                    order = table.Column<int>(type: "integer", nullable: false, comment: "排序"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, comment: "是否啟用"),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記"),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "建立者 ID"),
                    last_modification_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "最後修改時間"),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "最後修改者 ID"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "是否已刪除"),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "刪除者 ID"),
                    deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "刪除時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments", x => x.id);
                },
                comment: "部門");

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶Id"),
                    employee_no = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "員工編號"),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, comment: "姓名"),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "電子郵件"),
                    phone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true, comment: "電話"),
                    birth_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "生日"),
                    gender = table.Column<int>(type: "integer", nullable: false, comment: "性別"),
                    hire_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "到職日"),
                    termination_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "離職日"),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "員工狀態"),
                    department_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "所屬部門Id"),
                    position_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "職位Id"),
                    manager_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "直屬主管Id"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "使用者Id"),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記"),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "建立者 ID"),
                    last_modification_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "最後修改時間"),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "最後修改者 ID"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "是否已刪除"),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "刪除者 ID"),
                    deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "刪除時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.CheckConstraint("CK_employees_gender_Enum", "gender BETWEEN 1 AND 3");
                    table.CheckConstraint("CK_employees_status_Enum", "status BETWEEN 1 AND 3");
                },
                comment: "員工");

            migrationBuilder.CreateTable(
                name: "leave_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶Id"),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "員工Id"),
                    leave_type = table.Column<int>(type: "integer", nullable: false, comment: "假別"),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "開始日期"),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "結束日期"),
                    hours = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false, comment: "請假時數"),
                    reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "請假事由"),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "請假單狀態"),
                    approver_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "審核人Id"),
                    approved_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "審核時間"),
                    approver_comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "審核意見"),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記"),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "建立者 ID"),
                    last_modification_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "最後修改時間"),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "最後修改者 ID"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "是否已刪除"),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "刪除者 ID"),
                    deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "刪除時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_leave_requests", x => x.id);
                    table.CheckConstraint("CK_leave_requests_leave_type_Enum", "leave_type BETWEEN 1 AND 6");
                    table.CheckConstraint("CK_leave_requests_status_Enum", "status BETWEEN 1 AND 4");
                },
                comment: "請假單");

            migrationBuilder.CreateTable(
                name: "positions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶Id"),
                    code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, comment: "職位代碼"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "職位名稱"),
                    level = table.Column<int>(type: "integer", nullable: false, comment: "職等"),
                    base_salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "底薪"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, comment: "是否啟用"),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記"),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "建立者 ID"),
                    last_modification_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "最後修改時間"),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "最後修改者 ID"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "是否已刪除"),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "刪除者 ID"),
                    deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "刪除時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_positions", x => x.id);
                },
                comment: "職位");

            migrationBuilder.CreateTable(
                name: "salary_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶Id"),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false, comment: "員工Id"),
                    year = table.Column<int>(type: "integer", nullable: false, comment: "年"),
                    month = table.Column<int>(type: "integer", nullable: false, comment: "月"),
                    base_salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "底薪"),
                    overtime_pay = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "加班費"),
                    bonus = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "獎金"),
                    deductions = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "扣款"),
                    labor_insurance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "勞保"),
                    health_insurance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "健保"),
                    net_salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, comment: "實發薪資"),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "薪資狀態"),
                    note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "備註"),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記"),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "建立者 ID"),
                    last_modification_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "最後修改時間"),
                    last_modifier_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "最後修改者 ID"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "是否已刪除"),
                    deleter_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "刪除者 ID"),
                    deletion_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "刪除時間")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salary_records", x => x.id);
                    table.CheckConstraint("CK_salary_records_status_Enum", "status BETWEEN 1 AND 3");
                },
                comment: "薪資記錄");

            migrationBuilder.CreateIndex(
                name: "ix_attendance_records_date",
                table: "attendance_records",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "ix_attendance_records_employee_id",
                table: "attendance_records",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_attendance_records_employee_id_date",
                table: "attendance_records",
                columns: new[] { "employee_id", "date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_attendance_records_status",
                table: "attendance_records",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_departments_manager_id",
                table: "departments",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "ix_departments_parent_id",
                table: "departments",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_departments_tenant_id_code",
                table: "departments",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_department_id",
                table: "employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_manager_id",
                table: "employees",
                column: "manager_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_position_id",
                table: "employees",
                column: "position_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_status",
                table: "employees",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_employees_tenant_id_employee_no",
                table: "employees",
                columns: new[] { "tenant_id", "employee_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_user_id",
                table: "employees",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_leave_requests_approver_id",
                table: "leave_requests",
                column: "approver_id");

            migrationBuilder.CreateIndex(
                name: "ix_leave_requests_employee_id",
                table: "leave_requests",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_leave_requests_start_date_end_date",
                table: "leave_requests",
                columns: new[] { "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "ix_leave_requests_status",
                table: "leave_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_positions_tenant_id_code",
                table: "positions",
                columns: new[] { "tenant_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_salary_records_employee_id",
                table: "salary_records",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_salary_records_employee_id_year_month",
                table: "salary_records",
                columns: new[] { "employee_id", "year", "month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_salary_records_status",
                table: "salary_records",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendance_records");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "leave_requests");

            migrationBuilder.DropTable(
                name: "positions");

            migrationBuilder.DropTable(
                name: "salary_records");
        }
    }
}
