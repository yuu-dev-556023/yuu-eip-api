using Xunit;
using Yuu.Eip.Eips;

namespace Yuu.Eip.EntityFrameworkCore.Applications;

[Collection(EipTestConsts.CollectionDefinitionName)]
public class EfCoreEipAppServiceTests : EipAppServiceTests<EipEntityFrameworkCoreTestModule>
{

}
