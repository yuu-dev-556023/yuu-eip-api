using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;
using Yuu.Eip.Localization;

namespace Yuu.Eip;

[Dependency(ReplaceServices = true)]
public class EipBrandingProvider : DefaultBrandingProvider
{
    private readonly IStringLocalizer<EipResource> _localizer;

    public EipBrandingProvider(IStringLocalizer<EipResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
