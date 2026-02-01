using Volo.Abp.Modularity;

namespace Yuu.Eip;

public abstract class EipApplicationTestBase<TStartupModule> : EipTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
