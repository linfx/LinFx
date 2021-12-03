﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinFx.Extensions.Modularity;

/// <summary>
/// 模块
/// </summary>
public class Module : IModuleInitializer
{
    public virtual void ConfigureServices(IServiceCollection services)
    {
    }

    public virtual void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
    }
}
