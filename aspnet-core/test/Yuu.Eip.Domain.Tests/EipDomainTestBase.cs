using Volo.Abp.Modularity;

namespace Yuu.Eip;

/* Inherit from this class for your domain layer tests. */
public abstract class EipDomainTestBase<TStartupModule> : EipTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
