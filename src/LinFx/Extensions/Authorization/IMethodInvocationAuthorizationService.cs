﻿using System.Threading.Tasks;

namespace LinFx.Extensions.Authorization;

public interface IMethodInvocationAuthorizationService
{
    /// <summary>
    /// 校验
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task CheckAsync(MethodInvocationAuthorizationContext context);
}
