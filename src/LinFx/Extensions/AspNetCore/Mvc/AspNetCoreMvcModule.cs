using LinFx.Application;
using LinFx.Extensions.Modularity;
using Microsoft.AspNetCore.Mvc;

namespace LinFx.Extensions.AspNetCore.Mvc;

[DependsOn(
    typeof(AspNetCoreModule)
//typeof(LocalizationModule),
//typeof(AbpApiVersioningAbstractionsModule),
//typeof(AbpAspNetCoreMvcContractsModule),
//typeof(AbpUiNavigationModule),
//typeof(AbpGlobalFeaturesModule),
//typeof(AbpDddApplicationModule)
)]
public class AspNetCoreMvcModule : Module
{
    //public override void PreConfigureServices(ServiceConfigurationContext context)
    //{
    //    DynamicProxyIgnoreTypes.Add<ControllerBase>();
    //    DynamicProxyIgnoreTypes.Add<PageModel>();
    //    DynamicProxyIgnoreTypes.Add<ViewComponent>();

    //    context.Services.AddConventionalRegistrar(new AbpAspNetCoreMvcConventionalRegistrar());
    //}

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Configure<AbpApiDescriptionModelOptions>(options =>
        //{
        //    options.IgnoredInterfaces.AddIfNotContains(typeof(IAsyncActionFilter));
        //    options.IgnoredInterfaces.AddIfNotContains(typeof(IFilterMetadata));
        //    options.IgnoredInterfaces.AddIfNotContains(typeof(IActionFilter));
        //});

        //Configure<AbpRemoteServiceApiDescriptionProviderOptions>(options =>
        //{
        //    var statusCodes = new List<int>
        //    {
        //            (int) HttpStatusCode.Forbidden,
        //            (int) HttpStatusCode.Unauthorized,
        //            (int) HttpStatusCode.BadRequest,
        //            (int) HttpStatusCode.NotFound,
        //            (int) HttpStatusCode.NotImplemented,
        //            (int) HttpStatusCode.InternalServerError
        //    };

        //    options.SupportedResponseTypes.AddIfNotContains(statusCodes.Select(statusCode => new ApiResponseType
        //    {
        //        Type = typeof(RemoteServiceErrorResponse),
        //        StatusCode = statusCode
        //    }));
        //});

        //context.Services.PostConfigure<AbpAspNetCoreMvcOptions>(options =>
        //{
        //    if (options.MinifyGeneratedScript == null)
        //    {
        //        options.MinifyGeneratedScript = context.Services.GetHostingEnvironment().IsProduction();
        //    }
        //});

        //var mvcCoreBuilder = context.Services.AddMvcCore(options =>
        //{
        //    options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
        //});
        //context.Services.ExecutePreConfiguredActions(mvcCoreBuilder);

        //var abpMvcDataAnnotationsLocalizationOptions = context.Services
        //    .ExecutePreConfiguredActions(
        //        new AbpMvcDataAnnotationsLocalizationOptions()
        //    );

        //context.Services
        //    .AddSingleton<IOptions<AbpMvcDataAnnotationsLocalizationOptions>>(
        //        new OptionsWrapper<AbpMvcDataAnnotationsLocalizationOptions>(
        //            abpMvcDataAnnotationsLocalizationOptions
        //        )
        //    );

        //var mvcBuilder = context.Services.AddMvc()
        //    .AddRazorRuntimeCompilation()
        //    .AddDataAnnotationsLocalization(options =>
        //    {
        //        options.DataAnnotationLocalizerProvider = (type, factory) =>
        //        {
        //            var resourceType = abpMvcDataAnnotationsLocalizationOptions
        //                .AssemblyResources
        //                .GetOrDefault(type.Assembly);

        //            if (resourceType != null)
        //            {
        //                return factory.Create(resourceType);
        //            }

        //            return factory.CreateDefaultOrNull() ??
        //                   factory.Create(type);
        //        };
        //    })
        //    .AddViewLocalization(); //TODO: How to configure from the application? Also, consider to move to a UI module since APIs does not care about it.

        //mvcCoreBuilder.AddAbpHybridJson();

        //Configure<MvcRazorRuntimeCompilationOptions>(options =>
        //{
        //    options.FileProviders.Add(
        //        new RazorViewEngineVirtualFileProvider(
        //            context.Services.GetSingletonInstance<IObjectAccessor<IServiceProvider>>()
        //        )
        //    );
        //});

        //context.Services.ExecutePreConfiguredActions(mvcBuilder);

        //TODO: AddViewLocalization by default..?

        //context.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

        //Use DI to create controllers
        //mvcBuilder.AddControllersAsServices();

        //Use DI to create view components
        //mvcBuilder.AddViewComponentsAsServices();

        //Use DI to create razor page
        //context.Services.Replace(ServiceDescriptor.Singleton<IPageModelActivatorProvider, ServiceBasedPageModelActivatorProvider>());

        //Add feature providers
        //var partManager = context.Services.GetSingletonInstance<ApplicationPartManager>();
        //var application = context.Services.GetSingletonInstance<IApplication>();

        //partManager.FeatureProviders.Add(new AbpConventionalControllerFeatureProvider(application));
        //partManager.ApplicationParts.AddIfNotContains(typeof(AspNetCoreMvcModule).Assembly);

        //context.Services.Replace(ServiceDescriptor.Singleton<IValidationAttributeAdapterProvider, AbpValidationAttributeAdapterProvider>());
        //context.Services.AddSingleton<ValidationAttributeAdapterProvider>();

        Configure<MvcOptions>(mvcOptions =>
        {
            mvcOptions.AddMyOptions(context.Services);
        });

        //Configure<AbpEndpointRouterOptions>(options =>
        //{
        //    options.EndpointConfigureActions.Add(endpointContext =>
        //    {
        //        endpointContext.Endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
        //        endpointContext.Endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        //        endpointContext.Endpoints.MapRazorPages();
        //    });
        //});
    }

    //public override void PostConfigureServices(ServiceConfigurationContext context)
    //{
    //    ApplicationPartSorter.Sort(
    //        context.Services.GetSingletonInstance<ApplicationPartManager>(),
    //        context.Services.GetSingletonInstance<IModuleContainer>()
    //    );
    //}

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        //AddApplicationParts(context);
    }

    //private static void AddApplicationParts(ApplicationInitializationContext context)
    //{
    //    var partManager = context.ServiceProvider.GetService<ApplicationPartManager>();
    //    if (partManager == null)
    //    {
    //        return;
    //    }

    //    //Plugin modules
    //    var moduleAssemblies = context
    //        .ServiceProvider
    //        .GetRequiredService<IModuleContainer>()
    //        .Modules
    //        .Where(m => m.IsLoadedAsPlugIn)
    //        .Select(m => m.Type.Assembly)
    //        .Distinct();

    //    AddToApplicationParts(partManager, moduleAssemblies);

    //    //Controllers for application services
    //    var controllerAssemblies = context
    //        .ServiceProvider
    //        .GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>()
    //        .Value
    //        .ConventionalControllers
    //        .ConventionalControllerSettings
    //        .Select(s => s.Assembly)
    //        .Distinct();

    //    AddToApplicationParts(partManager, controllerAssemblies);
    //}

    //private static void AddToApplicationParts(ApplicationPartManager partManager, IEnumerable<Assembly> moduleAssemblies)
    //{
    //    foreach (var moduleAssembly in moduleAssemblies)
    //    {
    //        partManager.ApplicationParts.AddIfNotContains(moduleAssembly);
    //    }
    //}
}
