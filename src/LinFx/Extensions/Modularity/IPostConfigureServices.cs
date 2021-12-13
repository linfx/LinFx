namespace LinFx.Extensions.Modularity;

public interface IPostConfigureServices
{
    void PostConfigureServices(ServiceConfigurationContext context);
}
