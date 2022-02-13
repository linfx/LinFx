//using LinFx.Extensions.Timing;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Globalization;
//using System.Linq;

//namespace LinFx.Extensions.AspNetCore.Mvc.ModelBinding;

//public class DateTimeModelBinderProvider : IModelBinderProvider
//{
//    public IModelBinder GetBinder(ModelBinderProviderContext context)
//    {
//        var modelType = context.Metadata.UnderlyingOrModelType;
//        if (modelType == typeof(DateTime))
//        {
//            if (context.Metadata.ContainerType == null)
//            {
//                if (context.Metadata is DefaultModelMetadata defaultModelMetadata &&
//                    defaultModelMetadata.Attributes.Attributes.All(x => x.GetType() != typeof(DisableDateTimeNormalizationAttribute)))
//                {
//                    return CreateDateTimeModelBinder(context);
//                }
//            }
//            else
//            {
//                var dateNormalizationDisabledForClass =
//                    context.Metadata.ContainerType.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true);

//                var dateNormalizationDisabledForProperty = context.Metadata.ContainerType
//                    .GetProperty(context.Metadata.PropertyName)
//                    ?.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true);

//                if (!dateNormalizationDisabledForClass &&
//                    dateNormalizationDisabledForProperty != null &&
//                    !dateNormalizationDisabledForProperty.Value)
//                {
//                    return CreateDateTimeModelBinder(context);
//                }
//            }
//        }

//        return null;
//    }

//    protected virtual DateTimeModelBinder CreateDateTimeModelBinder(ModelBinderProviderContext context)
//    {
//        const DateTimeStyles supportedStyles = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AdjustToUniversal;
//        var dateTimeModelBinder = new DateTimeModelBinder(supportedStyles, context.Services.GetRequiredService<ILoggerFactory>());
//        return new DateTimeModelBinder(context.Services.GetRequiredService<IClock>(), dateTimeModelBinder);
//    }
//}
