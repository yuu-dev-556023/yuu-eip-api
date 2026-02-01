using Volo.Abp.Modularity;

namespace Yuu.Eip;

[DependsOn(
    typeof(EipDomainModule),
    typeof(EipTestBaseModule)
)]
public class EipDomainTestModule : AbpModule
{

}
