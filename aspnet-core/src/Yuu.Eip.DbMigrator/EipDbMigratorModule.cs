using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Yuu.Eip.EntityFrameworkCore;

namespace Yuu.Eip.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EipEntityFrameworkCoreModule),
    typeof(EipApplicationContractsModule)
    )]
public class EipDbMigratorModule : AbpModule
{
}
