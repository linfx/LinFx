using LinFx.Domain.Entities;
using LinFx.Extensions.Data;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.ExceptionHandling;
using LinFx.Extensions.ExceptionHandling.Localization;
using LinFx.Extensions.Http;
using LinFx.Extensions.Http.Client;
using LinFx.Extensions.Validation;
using LinFx.Security.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Text;

namespace LinFx.Extensions.AspNetCore.ExceptionHandling;

/// <summary>
/// 默认异常转换
/// </summary>
/// <param name="localizationOptions"></param>
/// <param name="stringLocalizerFactory"></param>
/// <param name="stringLocalizer"></param>
/// <param name="serviceProvider"></param>
public class DefaultExceptionToErrorInfoConverter(
    IOptions<ExceptionLocalizationOptions> localizationOptions,
    IStringLocalizerFactory stringLocalizerFactory,
    IStringLocalizer<ExceptionHandlingResource> stringLocalizer,
    IServiceProvider serviceProvider) : IExceptionToErrorInfoConverter, ITransientDependency
{
    protected ExceptionLocalizationOptions LocalizationOptions { get; } = localizationOptions.Value;

    protected IStringLocalizerFactory StringLocalizerFactory { get; } = stringLocalizerFactory;

    protected IStringLocalizer<ExceptionHandlingResource> L { get; } = stringLocalizer;

    protected IServiceProvider ServiceProvider { get; } = serviceProvider;

    public RemoteServiceErrorInfo Convert(Exception exception, Action<ExceptionHandlingOptions>? options = null)
    {
        var exceptionHandlingOptions = CreateDefaultOptions();
        options?.Invoke(exceptionHandlingOptions);

        var errorInfo = CreateErrorInfoWithoutCode(exception, exceptionHandlingOptions);

        if (exception is IHasErrorCode hasErrorCodeException)
            errorInfo.Code = hasErrorCodeException.Code;

        return errorInfo;
    }

    /// <summary>
    /// 创建错误信息
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    protected virtual RemoteServiceErrorInfo CreateErrorInfoWithoutCode(Exception exception, ExceptionHandlingOptions options)
    {
        if (options.SendExceptionsDetailsToClients)
            return CreateDetailedErrorInfoFromException(exception, options.SendStackTraceToClients);

        exception = TryToGetActualException(exception);

        // 远程调用异常
        if (exception is RemoteCallException remoteCallException && remoteCallException.Error != null)
            return remoteCallException.Error;

        // 数据库并发异常
        if (exception is DbConcurrencyException)
            return new RemoteServiceErrorInfo(L["DbConcurrencyErrorMessage"]);

        // 实体找不到异常
        if (exception is EntityNotFoundException)
            return CreateEntityNotFoundError((exception as EntityNotFoundException)!);

        var errorInfo = new RemoteServiceErrorInfo();

        if (exception is UserFriendlyException || exception is RemoteCallException)
        {
            errorInfo.Message = exception.Message;
            errorInfo.Details = (exception as IHasErrorDetails)?.Details!;
        }

        if (exception is IHasValidationErrors)
        {
            if (errorInfo.Message.IsNullOrEmpty())
                errorInfo.Message = L["ValidationErrorMessage"];

            if (errorInfo.Details.IsNullOrEmpty())
                errorInfo.Details = GetValidationErrorNarrative((exception as IHasValidationErrors)!);

            errorInfo.ValidationErrors = GetValidationErrorInfos((exception as IHasValidationErrors)!);
        }

        TryToLocalizeExceptionMessage(exception, errorInfo);

        if (errorInfo.Message.IsNullOrEmpty())
            errorInfo.Message = L["InternalServerErrorMessage"];

        errorInfo.Data = exception.Data;

        return errorInfo;
    }

    protected virtual void TryToLocalizeExceptionMessage(Exception exception, RemoteServiceErrorInfo errorInfo)
    {
        //if (exception is ILocalizeErrorMessage localizeErrorMessageException)
        //{
        //    using (var scope = ServiceProvider.CreateScope())
        //    {
        //        errorInfo.Message = localizeErrorMessageException.LocalizeMessage(new LocalizationContext(scope.ServiceProvider));
        //    }

        //    return;
        //}

        if (exception is not IHasErrorCode exceptionWithErrorCode)
            return;

        if (exceptionWithErrorCode.Code.IsNullOrWhiteSpace() || !exceptionWithErrorCode.Code.Contains(':'))
            return;

        var codeNamespace = exceptionWithErrorCode.Code.Split(':')[0];

        var localizationResourceType = LocalizationOptions.ErrorCodeNamespaceMappings.GetOrDefault(codeNamespace);
        if (localizationResourceType == null)
            return;

        var stringLocalizer = StringLocalizerFactory.Create(localizationResourceType);
        var localizedString = stringLocalizer[exceptionWithErrorCode.Code];
        if (localizedString.ResourceNotFound)
            return;

        var localizedValue = localizedString.Value;

        if (exception.Data != null && exception.Data.Count > 0)
        {
            foreach (var key in exception.Data.Keys)
            {
                localizedValue = localizedValue.Replace("{" + key + "}", exception.Data[key]?.ToString());
            }
        }

        errorInfo.Message = localizedValue;
    }

    /// <summary>
    /// 创建实体找不到错误信息
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected virtual RemoteServiceErrorInfo CreateEntityNotFoundError(EntityNotFoundException exception)
    {
        if (exception.EntityType != null)
            return new RemoteServiceErrorInfo(string.Format(L["EntityNotFoundErrorMessage"], exception.EntityType.Name, exception.Id));

        return new RemoteServiceErrorInfo(exception.Message);
    }

    protected virtual Exception TryToGetActualException(Exception exception)
    {
        if (exception is AggregateException && exception.InnerException != null)
        {
            var aggException = exception as AggregateException;

            if (aggException.InnerException is ValidationException ||
                aggException.InnerException is AuthorizationException ||
                aggException.InnerException is EntityNotFoundException ||
                aggException.InnerException is IBusinessException)
            {
                return aggException.InnerException;
            }
        }

        return exception;
    }

    protected virtual RemoteServiceErrorInfo CreateDetailedErrorInfoFromException(Exception exception, bool sendStackTraceToClients)
    {
        var detailBuilder = new StringBuilder();

        AddExceptionToDetails(exception, detailBuilder, sendStackTraceToClients);

        var errorInfo = new RemoteServiceErrorInfo(exception.Message, detailBuilder.ToString());

        if (exception is ValidationException)
            errorInfo.ValidationErrors = GetValidationErrorInfos(exception as ValidationException);

        return errorInfo;
    }

    protected virtual void AddExceptionToDetails(Exception exception, StringBuilder detailBuilder, bool sendStackTraceToClients)
    {
        //Exception Message
        detailBuilder.AppendLine(exception.GetType().Name + ": " + exception.Message);

        //Additional info for UserFriendlyException
        //if (exception is IUserFriendlyException &&
        //    exception is IHasErrorDetails)
        //{
        //    var details = ((IHasErrorDetails)exception).Details;
        //    if (!details.IsNullOrEmpty())
        //    {
        //        detailBuilder.AppendLine(details);
        //    }
        //}

        //Additional info for ValidationException
        if (exception is ValidationException)
        {
            var validationException = exception as ValidationException;
            if (validationException.ValidationErrors.Count > 0)
            {
                detailBuilder.AppendLine(GetValidationErrorNarrative(validationException));
            }
        }

        //Exception StackTrace
        if (sendStackTraceToClients && !string.IsNullOrEmpty(exception.StackTrace))
        {
            detailBuilder.AppendLine("STACK TRACE: " + exception.StackTrace);
        }

        //Inner exception
        if (exception.InnerException != null)
        {
            AddExceptionToDetails(exception.InnerException, detailBuilder, sendStackTraceToClients);
        }

        //Inner exceptions for AggregateException
        if (exception is AggregateException)
        {
            var aggException = exception as AggregateException;
            if (aggException.InnerExceptions.IsNullOrEmpty())
                return;

            foreach (var innerException in aggException.InnerExceptions)
            {
                AddExceptionToDetails(innerException, detailBuilder, sendStackTraceToClients);
            }
        }
    }

    protected virtual RemoteServiceValidationErrorInfo[] GetValidationErrorInfos(IHasValidationErrors validationException)
    {
        var validationErrorInfos = new List<RemoteServiceValidationErrorInfo>();

        foreach (var validationResult in validationException.ValidationErrors)
        {
            var validationError = new RemoteServiceValidationErrorInfo(validationResult.ErrorMessage);

            if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
            {
                validationError.Members = validationResult.MemberNames.Select(m => m.ToCamelCase()).ToArray();
            }

            validationErrorInfos.Add(validationError);
        }

        return [.. validationErrorInfos];
    }

    protected virtual string GetValidationErrorNarrative(IHasValidationErrors validationException)
    {
        var detailBuilder = new StringBuilder();
        detailBuilder.AppendLine(L["ValidationNarrativeErrorMessageTitle"]);

        foreach (var validationResult in validationException.ValidationErrors)
        {
            detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
            detailBuilder.AppendLine();
        }

        return detailBuilder.ToString();
    }

    protected virtual ExceptionHandlingOptions CreateDefaultOptions() => new ExceptionHandlingOptions
    {
        SendExceptionsDetailsToClients = false,
        SendStackTraceToClients = true
    };
}
