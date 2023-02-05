using FaturaTakip.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.RegularExpressions;

namespace FaturaTakip.ViewComponents;

public class CultureSwitcherViewComponent : ViewComponent
{
    private readonly IOptions<RequestLocalizationOptions> _localizationOptions;
    public CultureSwitcherViewComponent(IOptions<RequestLocalizationOptions> localizationOptions) =>
        _localizationOptions = localizationOptions;

    public IViewComponentResult Invoke()
    {
        var cultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
        var model = new CultureSwitcherModel
        {
            SupportedCultures = _localizationOptions.Value.SupportedUICultures.ToList(),
            CurrentUICulture = cultureFeature.RequestCulture.UICulture
        };

        return View(model);
    }
}