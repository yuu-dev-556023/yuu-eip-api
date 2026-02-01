using System.Threading.Tasks;

namespace Yuu.Eip.Data;

public interface IEipDbSchemaMigrator
{
    Task MigrateAsync();
}
