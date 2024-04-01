using LinFx.Extensions.Data;
using LinFx.Extensions.ExceptionHandling.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;

namespace IdentityService.Controllers;

[ApiController]
[Route("home")]
public class HomeController : ControllerBase
{
    // 用于提供 HomeController 的区域性资源
    private readonly IStringLocalizer _localizer;
    private readonly IStringLocalizer _sharedLocalizer;
    private readonly IStringLocalizer _sharedLocalizer2;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer3;

    public HomeController(
        IStringLocalizer<HomeController> localizer,
        IStringLocalizer<ExceptionHandlingResource> sharedLocalizer,
        IStringLocalizerFactory localizerFactory
    )
    {
        _localizer = localizerFactory.Create("Controllers.HomeController", Assembly.GetExecutingAssembly().FullName);
        //_sharedLocalizer = localizerFactory.Create("Controllers.HomeController", "IdentityService.Language"); 
        _sharedLocalizer2 = localizerFactory.Create(nameof(ExceptionHandlingResource), Assembly.GetExecutingAssembly().FullName);
        //_sharedLocalizer2 = sharedLocalizer;
    }

    [HttpGet]
    public IActionResult GetString([FromServices] IStringLocalizer<FunFeatureDefinitionProvider> funLocalizer)
    {
        var content = $"当前区域文化：{CultureInfo.CurrentCulture.Name}\n" +
            $"{_localizer["HelloWorld"]}\n" +
            //$"{_sharedLocalizer["HelloWorld"]}{DateTime.Now.ToLocalTime()}\n" +

           $"{funLocalizer["Fun.Sip"]}------------------{DateTime.Now.ToLocalTime()}\n" +


        $"{_sharedLocalizer2["CurrentTime"]}+++++++++++++{DateTime.Now.ToLocalTime()}\n";
        //return Content(content);

        throw new DbConcurrencyException();
    }
}
