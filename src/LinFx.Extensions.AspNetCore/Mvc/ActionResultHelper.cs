using LinFx.Extensions.Threading;
using Microsoft.AspNetCore.Mvc;

namespace LinFx.Extensions.AspNetCore.Mvc;

public static class ActionResultHelper
{
    public static List<Type> ObjectResultTypes { get; }

    static ActionResultHelper()
    {
        ObjectResultTypes =
        [
            typeof(JsonResult),
            typeof(ObjectResult),
            typeof(NoContentResult)
        ];
    }

    public static bool IsObjectResult(Type returnType, params Type[] excludeTypes)
    {
        returnType = AsyncHelper.UnwrapTask(returnType);

        if (!excludeTypes.IsNullOrEmpty() && excludeTypes.Any(t => t.IsAssignableFrom(returnType)))
            return false;

        if (!typeof(IActionResult).IsAssignableFrom(returnType))
            return true;

        return ObjectResultTypes.Any(t => t.IsAssignableFrom(returnType));
    }
}
