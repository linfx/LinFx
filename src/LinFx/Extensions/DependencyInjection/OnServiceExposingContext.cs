﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LinFx.Utils;

namespace LinFx.Extensions.DependencyInjection;

public class OnServiceExposingContext : IOnServiceExposingContext
{
    public Type ImplementationType { get; }

    public List<Type> ExposedTypes { get; }

    public OnServiceExposingContext([NotNull] Type implementationType, List<Type> exposedTypes)
    {
        ImplementationType = Check.NotNull(implementationType, nameof(implementationType));
        ExposedTypes = Check.NotNull(exposedTypes, nameof(exposedTypes));
    }
}
