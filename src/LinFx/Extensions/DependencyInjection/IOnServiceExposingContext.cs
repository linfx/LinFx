using System;
using System.Collections.Generic;

namespace LinFx.Extensions.DependencyInjection
{
    public interface IOnServiceExposingContext
    {
        Type ImplementationType { get; }

        List<Type> ExposedTypes { get; }
    }
}