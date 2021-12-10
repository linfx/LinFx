using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.DependencyInjection;

/// <summary>
/// 常规注册
/// </summary>
public interface IConventionalRegistrar
{
    /// <summary>
    /// 添加程序集
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    void AddAssembly(IServiceCollection services, Assembly assembly);

    /// <summary>
    /// 添加类型数组
    /// </summary>
    /// <param name="services"></param>
    /// <param name="types"></param>
    void AddTypes(IServiceCollection services, params Type[] types);

    /// <summary>
    /// 填加具体类型
    /// </summary>
    /// <param name="services"></param>
    /// <param name="type"></param>
    void AddType(IServiceCollection services, Type type);
}
