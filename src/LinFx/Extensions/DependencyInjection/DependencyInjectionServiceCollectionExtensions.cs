using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class DependencyInjectionServiceCollectionExtensions
{
    /// <summary>
    /// 注册程序集下实现依赖注入接口的类型
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="assembly"></param>
    public static LinFxBuilder AddAssembly(this LinFxBuilder builder, Assembly assembly)
    {
        builder.Services.AddAssembly(assembly);
        return builder;
    }
}