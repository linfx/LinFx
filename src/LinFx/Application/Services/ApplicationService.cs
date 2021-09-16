using LinFx.Data;
using LinFx.Extensions.Auditing;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.Guids;
using LinFx.Extensions.MultiTenancy;
using LinFx.Extensions.ObjectMapping;
using LinFx.Extensions.Setting;
using LinFx.Extensions.Timing;
using LinFx.Extensions.Uow;
using LinFx.Linq;
using LinFx.Security.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;

namespace LinFx.Application.Services
{
    [Service]
    public abstract class ApplicationService :
        IApplicationService,
        //IAvoidDuplicateCrossCuttingConcerns,
        //IValidationEnabled,
        IUnitOfWorkEnabled,
        IAuditingEnabled
        //IGlobalFeatureCheckingEnabled
    {
        public ILazyServiceProvider LazyServiceProvider { get; private set; }

        public static string[] CommonPostfixes { get; set; } = { "AppService", "ApplicationService", "Service" };

        public List<string> AppliedCrossCuttingConcerns { get; } = new List<string>();

        protected IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

        protected IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

        protected Type ObjectMapperContext { get; set; }
        protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
            ObjectMapperContext == null
                ? provider.GetRequiredService<IObjectMapper>()
                : provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));

        protected IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

        protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

        protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

        protected IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

        protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();

        protected ISettingProvider SettingProvider => LazyServiceProvider.LazyGetRequiredService<ISettingProvider>();

        protected IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

        protected IAuthorizationService AuthorizationService => LazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

        //protected IFeatureChecker FeatureChecker => LazyServiceProvider.LazyGetRequiredService<IFeatureChecker>();

        protected IStringLocalizerFactory StringLocalizerFactory => LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();

        protected ApplicationService(IServiceProvider serviceProvider)
        {
            LazyServiceProvider = serviceProvider.GetRequiredService<ILazyServiceProvider>();
        }

        protected IStringLocalizer L
        {
            get
            {
                if (_localizer == null)
                {
                    //_localizer = CreateLocalizer();
                }

                return _localizer;
            }
        }
        private IStringLocalizer _localizer;

        //protected Type LocalizationResource
        //{
        //    get => _localizationResource;
        //    set
        //    {
        //        _localizationResource = value;
        //        _localizer = null;
        //    }
        //}
        //private Type _localizationResource = typeof(DefaultResource);

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance);

        ///// <summary>
        ///// Checks for given <paramref name="policyName"/>.
        ///// Throws <see cref="AuthorizationException"/> if given policy has not been granted.
        ///// </summary>
        ///// <param name="policyName">The policy name. This method does nothing if given <paramref name="policyName"/> is null or empty.</param>
        //protected virtual async Task CheckPolicyAsync([CanBeNull] string policyName)
        //{
        //    if (string.IsNullOrEmpty(policyName))
        //    {
        //        return;
        //    }

        //    await AuthorizationService.CheckAsync(policyName);
        //}

        //protected virtual IStringLocalizer CreateLocalizer()
        //{
        //    if (LocalizationResource != null)
        //    {
        //        return StringLocalizerFactory.Create(LocalizationResource);
        //    }

        //    var localizer = StringLocalizerFactory.CreateDefaultOrNull();
        //    if (localizer == null)
        //    {
        //        throw new Exception($"Set {nameof(LocalizationResource)} or define the default localization resource type (by configuring the {nameof(AbpLocalizationOptions)}.{nameof(AbpLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");
        //    }

        //    return localizer;
        //}
    }
}
