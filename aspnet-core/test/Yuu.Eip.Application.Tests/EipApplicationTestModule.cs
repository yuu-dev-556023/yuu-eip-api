using Volo.Abp.Modularity;

namespace Yuu.Eip;

[DependsOn(
    typeof(EipApplicationModule),
    typeof(EipDomainTestModule)
)]
public class EipApplicationTestModule : AbpModule
{

}
