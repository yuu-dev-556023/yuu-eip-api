using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yuu.Eip.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_log_excel_files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    file_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "建立者 ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_log_excel_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    application_name = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    tenant_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    impersonator_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    impersonator_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    impersonator_tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    impersonator_tenant_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    execution_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    execution_duration = table.Column<int>(type: "integer", nullable: false),
                    client_ip_address = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    client_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    client_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    correlation_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    browser_info = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    http_method = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    exceptions = table.Column<string>(type: "text", nullable: true),
                    comments = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    http_status_code = table.Column<int>(type: "integer", nullable: true),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "background_jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    application_name = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
                    job_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    job_args = table.Column<string>(type: "character varying(1048576)", maxLength: 1048576, nullable: false),
                    try_count = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    next_try_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_try_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_abandoned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    priority = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)15),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_background_jobs", x => x.id);
                    table.CheckConstraint("CK_background_jobs_priority_Enum", "priority IN (5, 10, 15, 20, 25)");
                });

            migrationBuilder.CreateTable(
                name: "claim_types",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    required = table.Column<bool>(type: "boolean", nullable: false),
                    is_static = table.Column<bool>(type: "boolean", nullable: false),
                    regex = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    regex_description = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    value_type = table.Column<int>(type: "integer", nullable: false),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_claim_types", x => x.id);
                    table.CheckConstraint("CK_claim_types_value_type_Enum", "value_type BETWEEN 0 AND 3");
                });

            migrationBuilder.CreateTable(
                name: "feature_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    display_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    extra_properties = table.Column<string>(type: "text", nullable: true, comment: "擴充屬性")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feature_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "feature_values",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    value = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    provider_key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feature_values", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "features",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    group_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    parent_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    display_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    default_value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    is_visible_to_clients = table.Column<bool>(type: "boolean", nullable: false),
                    is_available_to_host = table.Column<bool>(type: "boolean", nullable: false),
                    allowed_providers = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    value_type = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    extra_properties = table.Column<string>(type: "text", nullable: true, comment: "擴充屬性")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_features", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "link_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    source_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    source_tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    target_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    target_tenant_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_link_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "open_iddict_applications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    application_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    client_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_secret = table.Column<string>(type: "text", nullable: true),
                    client_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    consent_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    display_names = table.Column<string>(type: "text", nullable: true),
                    json_web_key_set = table.Column<string>(type: "text", nullable: true),
                    permissions = table.Column<string>(type: "text", nullable: true),
                    post_logout_redirect_uris = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    redirect_uris = table.Column<string>(type: "text", nullable: true),
                    requirements = table.Column<string>(type: "text", nullable: true),
                    settings = table.Column<string>(type: "text", nullable: true),
                    front_channel_logout_uri = table.Column<string>(type: "text", nullable: true),
                    client_uri = table.Column<string>(type: "text", nullable: true),
                    logo_uri = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("pk_open_iddict_applications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "open_iddict_scopes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptions = table.Column<string>(type: "text", nullable: true),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    display_names = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    resources = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("pk_open_iddict_scopes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "organization_units",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    code = table.Column<string>(type: "character varying(95)", maxLength: 95, nullable: false),
                    display_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    entity_version = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("pk_organization_units", x => x.id);
                    table.ForeignKey(
                        name: "fk_organization_units_organization_units_parent_id",
                        column: x => x.parent_id,
                        principalTable: "organization_units",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "permission_grants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    provider_key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_grants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    display_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    extra_properties = table.Column<string>(type: "text", nullable: true, comment: "擴充屬性")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    group_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    parent_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    display_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    multi_tenancy_side = table.Column<byte>(type: "smallint", nullable: false),
                    providers = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    state_checkers = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    extra_properties = table.Column<string>(type: "text", nullable: true, comment: "擴充屬性")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    is_static = table.Column<bool>(type: "boolean", nullable: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    entity_version = table.Column<int>(type: "integer", nullable: false),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "security_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    application_name = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
                    identity = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
                    action = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    tenant_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    client_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    correlation_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    client_ip_address = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    browser_info = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_security_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    session_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    device = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    device_info = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    client_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ip_addresses = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    signed_in = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_accessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    extra_properties = table.Column<string>(type: "text", nullable: true, comment: "擴充屬性")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sessions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "setting_definitions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    display_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    default_value = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    is_visible_to_clients = table.Column<bool>(type: "boolean", nullable: false),
                    providers = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    is_inherited = table.Column<bool>(type: "boolean", nullable: false),
                    is_encrypted = table.Column<bool>(type: "boolean", nullable: false),
                    extra_properties = table.Column<string>(type: "text", nullable: true, comment: "擴充屬性")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_setting_definitions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    value = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    provider_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    provider_key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    entity_version = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("pk_tenants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_delegations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    source_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    target_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_delegations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    surname = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    password_hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    security_stamp = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    is_external = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    phone_number = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    should_change_password_on_next_login = table.Column<bool>(type: "boolean", nullable: false),
                    entity_version = table.Column<int>(type: "integer", nullable: false),
                    last_password_change_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "audit_log_actions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    audit_log_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    method_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    parameters = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    execution_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    execution_duration = table.Column<int>(type: "integer", nullable: false),
                    extra_properties = table.Column<string>(type: "text", nullable: true, comment: "擴充屬性")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_log_actions", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_log_actions_audit_logs_audit_log_id",
                        column: x => x.audit_log_id,
                        principalTable: "audit_logs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "entity_changes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    audit_log_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    change_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    change_type = table.Column<byte>(type: "smallint", nullable: false),
                    entity_tenant_id = table.Column<Guid>(type: "uuid", nullable: true),
                    entity_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    entity_type_full_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    extra_properties = table.Column<string>(type: "text", nullable: true, comment: "擴充屬性")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_entity_changes", x => x.id);
                    table.CheckConstraint("CK_entity_changes_change_type_Enum", "change_type BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "fk_entity_changes_audit_logs_audit_log_id",
                        column: x => x.audit_log_id,
                        principalTable: "audit_logs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "open_iddict_authorizations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    application_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    scopes = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    subject = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_authorizations", x => x.id);
                    table.ForeignKey(
                        name: "fk_open_iddict_authorizations_open_iddict_applications_applica~",
                        column: x => x.application_id,
                        principalTable: "open_iddict_applications",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "organization_unit_roles",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_unit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "建立者 ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organization_unit_roles", x => new { x.organization_unit_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_organization_unit_roles_organization_units_organization_uni~",
                        column: x => x.organization_unit_id,
                        principalTable: "organization_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organization_unit_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_claims",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    claim_type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    claim_value = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_claims_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tenant_connection_strings",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    value = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenant_connection_strings", x => new { x.tenant_id, x.name });
                    table.ForeignKey(
                        name: "fk_tenant_connection_strings_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claims",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    claim_type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    claim_value = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_claims_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    provider_key = table.Column<string>(type: "character varying(196)", maxLength: 196, nullable: false),
                    provider_display_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_logins", x => new { x.user_id, x.login_provider });
                    table.ForeignKey(
                        name: "fk_user_logins_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_organization_units",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_unit_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "建立時間"),
                    creator_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "建立者 ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_organization_units", x => new { x.organization_unit_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_user_organization_units__organization_units_organization_unit",
                        column: x => x.organization_unit_id,
                        principalTable: "organization_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_organization_units_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_user_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "entity_property_changes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: true, comment: "租戶 ID"),
                    entity_change_id = table.Column<Guid>(type: "uuid", nullable: false),
                    new_value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    original_value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    property_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    property_type_full_name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_entity_property_changes", x => x.id);
                    table.ForeignKey(
                        name: "fk_entity_property_changes_entity_changes_entity_change_id",
                        column: x => x.entity_change_id,
                        principalTable: "entity_changes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "open_iddict_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, comment: "主鍵"),
                    application_id = table.Column<Guid>(type: "uuid", nullable: true),
                    authorization_id = table.Column<Guid>(type: "uuid", nullable: true),
                    creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    expiration_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    payload = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    redemption_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    reference_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    subject = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    type = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    extra_properties = table.Column<string>(type: "text", nullable: false, comment: "擴充屬性"),
                    concurrency_stamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false, comment: "併發戳記")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_open_iddict_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_open_iddict_tokens_open_iddict_applications_application_id",
                        column: x => x.application_id,
                        principalTable: "open_iddict_applications",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_open_iddict_tokens_open_iddict_authorizations_authorization~",
                        column: x => x.authorization_id,
                        principalTable: "open_iddict_authorizations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_audit_log_actions_audit_log_id",
                table: "audit_log_actions",
                column: "audit_log_id");

            migrationBuilder.CreateIndex(
                name: "ix_audit_log_actions_tenant_id_service_name_method_name_execut~",
                table: "audit_log_actions",
                columns: new[] { "tenant_id", "service_name", "method_name", "execution_time" });

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_tenant_id_execution_time",
                table: "audit_logs",
                columns: new[] { "tenant_id", "execution_time" });

            migrationBuilder.CreateIndex(
                name: "ix_audit_logs_tenant_id_user_id_execution_time",
                table: "audit_logs",
                columns: new[] { "tenant_id", "user_id", "execution_time" });

            migrationBuilder.CreateIndex(
                name: "ix_background_jobs_is_abandoned_next_try_time",
                table: "background_jobs",
                columns: new[] { "is_abandoned", "next_try_time" });

            migrationBuilder.CreateIndex(
                name: "ix_entity_changes_audit_log_id",
                table: "entity_changes",
                column: "audit_log_id");

            migrationBuilder.CreateIndex(
                name: "ix_entity_changes_tenant_id_entity_type_full_name_entity_id",
                table: "entity_changes",
                columns: new[] { "tenant_id", "entity_type_full_name", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "ix_entity_property_changes_entity_change_id",
                table: "entity_property_changes",
                column: "entity_change_id");

            migrationBuilder.CreateIndex(
                name: "ix_feature_groups_name",
                table: "feature_groups",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_feature_values_name_provider_name_provider_key",
                table: "feature_values",
                columns: new[] { "name", "provider_name", "provider_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_features_group_name",
                table: "features",
                column: "group_name");

            migrationBuilder.CreateIndex(
                name: "ix_features_name",
                table: "features",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_link_users_source_user_id_source_tenant_id_target_user_id_t~",
                table: "link_users",
                columns: new[] { "source_user_id", "source_tenant_id", "target_user_id", "target_tenant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_applications_client_id",
                table: "open_iddict_applications",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_authorizations_application_id_status_subject_ty~",
                table: "open_iddict_authorizations",
                columns: new[] { "application_id", "status", "subject", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_scopes_name",
                table: "open_iddict_scopes",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_tokens_application_id_status_subject_type",
                table: "open_iddict_tokens",
                columns: new[] { "application_id", "status", "subject", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_tokens_authorization_id",
                table: "open_iddict_tokens",
                column: "authorization_id");

            migrationBuilder.CreateIndex(
                name: "ix_open_iddict_tokens_reference_id",
                table: "open_iddict_tokens",
                column: "reference_id");

            migrationBuilder.CreateIndex(
                name: "ix_organization_unit_roles_role_id_organization_unit_id",
                table: "organization_unit_roles",
                columns: new[] { "role_id", "organization_unit_id" });

            migrationBuilder.CreateIndex(
                name: "ix_organization_units_code",
                table: "organization_units",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "ix_organization_units_parent_id",
                table: "organization_units",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_permission_grants_tenant_id_name_provider_name_provider_key",
                table: "permission_grants",
                columns: new[] { "tenant_id", "name", "provider_name", "provider_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_permission_groups_name",
                table: "permission_groups",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_permissions_group_name",
                table: "permissions",
                column: "group_name");

            migrationBuilder.CreateIndex(
                name: "ix_permissions_name",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_role_claims_role_id",
                table: "role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_normalized_name",
                table: "roles",
                column: "normalized_name");

            migrationBuilder.CreateIndex(
                name: "ix_security_logs_tenant_id_action",
                table: "security_logs",
                columns: new[] { "tenant_id", "action" });

            migrationBuilder.CreateIndex(
                name: "ix_security_logs_tenant_id_application_name",
                table: "security_logs",
                columns: new[] { "tenant_id", "application_name" });

            migrationBuilder.CreateIndex(
                name: "ix_security_logs_tenant_id_identity",
                table: "security_logs",
                columns: new[] { "tenant_id", "identity" });

            migrationBuilder.CreateIndex(
                name: "ix_security_logs_tenant_id_user_id",
                table: "security_logs",
                columns: new[] { "tenant_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_sessions_device",
                table: "sessions",
                column: "device");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_session_id",
                table: "sessions",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_sessions_tenant_id_user_id",
                table: "sessions",
                columns: new[] { "tenant_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_setting_definitions_name",
                table: "setting_definitions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_settings_name_provider_name_provider_key",
                table: "settings",
                columns: new[] { "name", "provider_name", "provider_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tenants_name",
                table: "tenants",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_tenants_normalized_name",
                table: "tenants",
                column: "normalized_name");

            migrationBuilder.CreateIndex(
                name: "ix_user_claims_user_id",
                table: "user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_login_provider_provider_key",
                table: "user_logins",
                columns: new[] { "login_provider", "provider_key" });

            migrationBuilder.CreateIndex(
                name: "ix_user_organization_units_user_id_organization_unit_id",
                table: "user_organization_units",
                columns: new[] { "user_id", "organization_unit_id" });

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id_user_id",
                table: "user_roles",
                columns: new[] { "role_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "ix_users_normalized_email",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "ix_users_normalized_user_name",
                table: "users",
                column: "normalized_user_name");

            migrationBuilder.CreateIndex(
                name: "ix_users_user_name",
                table: "users",
                column: "user_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_log_actions");

            migrationBuilder.DropTable(
                name: "audit_log_excel_files");

            migrationBuilder.DropTable(
                name: "background_jobs");

            migrationBuilder.DropTable(
                name: "claim_types");

            migrationBuilder.DropTable(
                name: "entity_property_changes");

            migrationBuilder.DropTable(
                name: "feature_groups");

            migrationBuilder.DropTable(
                name: "feature_values");

            migrationBuilder.DropTable(
                name: "features");

            migrationBuilder.DropTable(
                name: "link_users");

            migrationBuilder.DropTable(
                name: "open_iddict_scopes");

            migrationBuilder.DropTable(
                name: "open_iddict_tokens");

            migrationBuilder.DropTable(
                name: "organization_unit_roles");

            migrationBuilder.DropTable(
                name: "permission_grants");

            migrationBuilder.DropTable(
                name: "permission_groups");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "role_claims");

            migrationBuilder.DropTable(
                name: "security_logs");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "setting_definitions");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "tenant_connection_strings");

            migrationBuilder.DropTable(
                name: "user_claims");

            migrationBuilder.DropTable(
                name: "user_delegations");

            migrationBuilder.DropTable(
                name: "user_logins");

            migrationBuilder.DropTable(
                name: "user_organization_units");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "user_tokens");

            migrationBuilder.DropTable(
                name: "entity_changes");

            migrationBuilder.DropTable(
                name: "open_iddict_authorizations");

            migrationBuilder.DropTable(
                name: "tenants");

            migrationBuilder.DropTable(
                name: "organization_units");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "open_iddict_applications");
        }
    }
}
