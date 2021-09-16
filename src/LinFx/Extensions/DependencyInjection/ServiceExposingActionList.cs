using System;
using System.Collections.Generic;

namespace LinFx.Extensions.DependencyInjection
{
    public class ServiceExposingActionList : List<Action<IOnServiceExposingContext>>
    {
    }
}