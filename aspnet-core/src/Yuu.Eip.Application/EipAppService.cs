using Volo.Abp.Application.Services;
using Yuu.Eip.Localization;

namespace Yuu.Eip;

/* Inherit your application services from this class.
 */
public abstract class EipAppService : ApplicationService
{
    protected EipAppService()
    {
        LocalizationResource = typeof(EipResource);
    }
}
