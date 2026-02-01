using Xunit;

namespace Yuu.Eip.EntityFrameworkCore;

[CollectionDefinition(EipTestConsts.CollectionDefinitionName)]
public class EipEntityFrameworkCoreCollection : ICollectionFixture<EipEntityFrameworkCoreFixture>
{

}
