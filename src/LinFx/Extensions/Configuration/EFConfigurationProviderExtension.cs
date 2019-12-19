using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace LinFx.Extensions.Configuration
{
    public static class EFConfigurationProviderExtension
    {
        public static IConfigurationBuilder AddEntityFrameworkConfig(this IConfigurationBuilder builder, Action<DbContextOptionsBuilder> setup)
        {
            return builder.Add(new EFConfigSource(setup));
        }
    }
}
