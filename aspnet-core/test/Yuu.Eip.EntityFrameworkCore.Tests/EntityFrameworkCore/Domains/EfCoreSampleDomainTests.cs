using Xunit;
using Yuu.Eip.Eips;

namespace Yuu.Eip.EntityFrameworkCore.Domains;

[Collection(EipTestConsts.CollectionDefinitionName)]
public class EfCoreEipDomainTests : EipDomainTests<EipEntityFrameworkCoreTestModule>
{

}
