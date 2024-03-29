using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace IdentityService.Controllers;

[ApiController]
[Route("home")]
public class HomeController : ControllerBase
{
    // 用于提供 HomeController 的区域性资源
    private readonly IStringLocalizer _localizer;

    // 通过代理伪类提供共享资源
    private readonly IStringLocalizer _sharedLocalizer;

    public HomeController(
        IStringLocalizer<HomeController> localizer,
        IStringLocalizer<SharedResource> sharedLocalizer
    )
    {
        _localizer = localizer;
        _sharedLocalizer = sharedLocalizer;
    }

    [HttpGet]
    public IActionResult GetString()
    {
        var content = $"当前区域文化：{CultureInfo.CurrentCulture.Name}\n" +
            $"{_localizer["HelloWorld"]}\n" +
            $"{_sharedLocalizer["CurrentTime"]}{DateTime.Now.ToLocalTime()}\n";
        return Content(content);
    }
}
