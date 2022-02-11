using LinFx.Extensions.DynamicProxy;
using LinFx.Extensions.Modularity;

namespace LinFx.Extensions.Autofac;

[DependsOn(typeof(CastleCoreModule))]
public class AutofacModule : Module
{
}
