using LinFx.Extensions.Auditing;
using LinFx.Extensions.Authorization;
using LinFx.Extensions.Data;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Features;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.ObjectMapping;
using LinFx.Extensions.Setting;
using LinFx.Extensions.Uow;
using LinFx.Linq;
using LinFx.Security.Authorization;
using LinFx.Security.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Application.Services;

/// <summary>
/// Ӧ�÷���
/// </summary>
public abstract class ApplicationService :
    IApplicationService,
    IAuditingEnabled,
    ITransientDependency
{
    [NotNull]
    [Autowired]
    public ILazyServiceProvider? LazyServiceProvider { get; set; }

    public static string[] CommonPostfixes { get; set; } = ["ApplicationService", "Service"];

    public List<string> AppliedCrossCuttingConcerns { get; } = [];

    /// <summary>
    /// ������Ԫ������
    /// </summary>
    protected IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    /// <summary>
    /// �첽ִ����
    /// </summary>
    protected IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

    protected Type? ObjectMapperContext { get; set; }

    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider => ObjectMapperContext == null
            ? provider.GetRequiredService<IObjectMapper>()
            : provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));

    /// <summary>
    /// ��־����
    /// </summary>
    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    /// <summary>
    /// ��ǰ�⻧
    /// </summary>
    protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    /// <summary>
    /// ���ݹ���
    /// </summary>
    protected IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    /// <summary>
    /// ��ǰ�û�
    /// </summary>
    protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();

    /// <summary>
    /// ����
    /// </summary>
    protected ISettingProvider SettingProvider => LazyServiceProvider.LazyGetRequiredService<ISettingProvider>();

    /// <summary>
    /// ��Ȩ����
    /// </summary>
    protected IAuthorizationService AuthorizationService => LazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

    /// <summary>
    /// ���������
    /// </summary>
    protected IFeatureChecker FeatureChecker => LazyServiceProvider.LazyGetRequiredService<IFeatureChecker>();

    public ApplicationService() { }

    public ApplicationService(IServiceProvider serviceProvider) => LazyServiceProvider = serviceProvider.GetRequiredService<ILazyServiceProvider>();

    protected IStringLocalizer L
    {
        get
        {
            if (_localizer == null)
            {
                _localizer = LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>().Create(LocalizationResource);
            }
            return _localizer;
        }
    }
    private IStringLocalizer? _localizer;

    protected Type LocalizationResource
    {
        get => _localizationResource ?? GetType();
        set
        {
            _localizationResource = value;
            _localizer = null;
        }
    }
    private Type? _localizationResource;

    /// <summary>
    /// ��ǰ������Ԫ
    /// </summary>
    protected IUnitOfWork? CurrentUnitOfWork => UnitOfWorkManager?.Current;

    /// <summary>
    /// ��־
    /// </summary>
    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance);

    /// <summary>
    /// Checks for given <paramref name="policyName"/>.
    /// Throws <see cref="AuthorizationException"/> if given policy has not been granted.
    /// </summary>
    /// <param name="policyName">The policy name. This method does nothing if given <paramref name="policyName"/> is null or empty.</param>
    protected virtual async Task CheckPolicyAsync(string policyName)
    {
        if (string.IsNullOrEmpty(policyName))
            return;

        await AuthorizationService.CheckAsync(policyName);
    }
}
