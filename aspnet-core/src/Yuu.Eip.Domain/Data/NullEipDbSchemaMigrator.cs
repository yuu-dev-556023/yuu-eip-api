using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Yuu.Eip.Data;

/* This is used if database provider does't define
 * IEipDbSchemaMigrator implementation.
 */
public class NullEipDbSchemaMigrator : IEipDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
