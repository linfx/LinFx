using System;
using System.Collections.Generic;

namespace LinFx.Extensions.DependencyInjection
{
    public class ServiceRegistrationActionList : List<Action<IOnServiceRegistredContext>>
    {
    }
}