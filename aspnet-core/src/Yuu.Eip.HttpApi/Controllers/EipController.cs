using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Yuu.Eip.Localization;

namespace Yuu.Eip.Controllers;

/* Inherit your controllers from this class.
 */
[RemoteService(Name = "Eip")]
[Route("api/v1/[controller]")]
[Authorize]
public abstract class EipController : AbpControllerBase
{
    protected EipController()
    {
        LocalizationResource = typeof(EipResource);
    }
}
