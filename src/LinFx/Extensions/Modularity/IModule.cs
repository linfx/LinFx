namespace LinFx.Extensions.Modularity;

public interface IModule
{
    void ConfigureServices(ServiceConfigurationContext context);
}