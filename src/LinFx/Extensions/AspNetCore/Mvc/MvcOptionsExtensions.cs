using LinFx.Extensions.AspNetCore.Mvc.Auditing;
using LinFx.Extensions.AspNetCore.Mvc.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.AspNetCore.Mvc;

internal static class MvcOptionsExtensions
{
    public static void AddOptions(this MvcOptions options, IServiceCollection services)
    {
        AddConventions(options, services);
        AddActionFilters(options);
        AddModelBinders(options);
        AddMetadataProviders(options, services);
        AddFormatters(options);
    }

    private static void AddFormatters(MvcOptions options)
    {
        //options.OutputFormatters.Insert(0, new RemoteStreamContentOutputFormatter());
    }

    private static void AddConventions(MvcOptions options, IServiceCollection services)
    {
        //options.Conventions.Add(new AbpServiceConventionWrapper(services));
    }

    /// <summary>
    /// 注入过滤器
    /// </summary>
    /// <param name="options"></param>
    private static void AddActionFilters(MvcOptions options)
    {
        //options.Filters.AddService(typeof(GlobalFeatureActionFilter));
        options.Filters.AddService(typeof(AuditActionFilter));            // 注入审计日志过滤器
        //options.Filters.AddService(typeof(AbpNoContentActionFilter));
        //options.Filters.AddService(typeof(AbpFeatureActionFilter));
        //options.Filters.AddService(typeof(AbpValidationActionFilter));
        options.Filters.AddService(typeof(UowActionFilter));
        //options.Filters.AddService(typeof(AbpExceptionFilter));
    }

    private static void AddModelBinders(MvcOptions options)
    {
        //options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
        //options.ModelBinderProviders.Insert(1, new AbpExtraPropertiesDictionaryModelBinderProvider());
        //options.ModelBinderProviders.Insert(2, new AbpRemoteStreamContentModelBinderProvider());
    }

    private static void AddMetadataProviders(MvcOptions options, IServiceCollection services)
    {
        //options.ModelMetadataDetailsProviders.Add(new AbpDataAnnotationAutoLocalizationMetadataDetailsProvider(services));

        //options.ModelMetadataDetailsProviders.Add(new BindingSourceMetadataProvider(typeof(IRemoteStreamContent), BindingSource.FormFile));
        //options.ModelMetadataDetailsProviders.Add(new BindingSourceMetadataProvider(typeof(IEnumerable<IRemoteStreamContent>), BindingSource.FormFile));
        //options.ModelMetadataDetailsProviders.Add(new BindingSourceMetadataProvider(typeof(RemoteStreamContent), BindingSource.FormFile));
        //options.ModelMetadataDetailsProviders.Add(new BindingSourceMetadataProvider(typeof(IEnumerable<RemoteStreamContent>), BindingSource.FormFile));
        //options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(IRemoteStreamContent)));
        //options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(RemoteStreamContent)));
    }
}
