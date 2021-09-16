﻿using LinFx.Extensions.MultiTenancy;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MultiTenancyServiceCollectionExtensions
    {
        public static LinFxBuilder AddMultiTenancy(this LinFxBuilder builder, Action<MultiTenancyOptions> optionsAction = default)
        {
            if(optionsAction != null)
            {
                builder.Services.Configure(optionsAction);
            }

            return builder;
        }
    }
}
