using LinFx;
using LinFx.Domain.Entities;
using LinFx.Extensions.AspNetCore.ExceptionHandling;
using LinFx.Extensions.Data;
using LinFx.Extensions.DependencyInjection;
using LinFx.Extensions.ExceptionHandling;
using LinFx.Extensions.ExceptionHandling.Localization;
using LinFx.Extensions.Http;
using LinFx.Extensions.Http.Client;
using LinFx.Extensions.Validation;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace IdentityService;

/// <summary>
/// 异常转换
/// </summary>
[Service(ReplaceServices = true)]
public class ExceptionToErrorInfoConverter : DefaultExceptionToErrorInfoConverter, IExceptionToErrorInfoConverter
{
    protected override IStringLocalizer L => StringLocalizerFactory.Create(nameof(ExceptionHandlingResource), Assembly.GetExecutingAssembly().FullName);

    protected override RemoteServiceErrorInfo CreateErrorInfoWithoutCode(Exception exception, ExceptionHandlingOptions options)
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
            if (string.IsNullOrEmpty(errorInfo.Message))
                errorInfo.Message = L["ValidationErrorMessage"];

            if (string.IsNullOrEmpty(errorInfo.Details))
                errorInfo.Details = GetValidationErrorNarrative((exception as IHasValidationErrors)!);

            errorInfo.ValidationErrors = GetValidationErrorInfos((exception as IHasValidationErrors)!);
        }

        TryToLocalizeExceptionMessage(exception, errorInfo);

        if (string.IsNullOrEmpty(errorInfo.Message))
            errorInfo.Message = L["InternalServerErrorMessage"];

        errorInfo.Data = exception.Data;

        return errorInfo;
    }
}