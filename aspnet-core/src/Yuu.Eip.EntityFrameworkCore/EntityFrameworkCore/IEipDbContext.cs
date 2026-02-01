using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Yuu.Eip.EntityFrameworkCore;

public interface IEipDbContext :
    IIdentityDbContext,
    ITenantManagementDbContext,
    IPermissionManagementDbContext,
    ISettingManagementDbContext,
    IBackgroundJobsDbContext,
    IAuditLoggingDbContext,
    IOpenIddictDbContext,
    IFeatureManagementDbContext
{
}
