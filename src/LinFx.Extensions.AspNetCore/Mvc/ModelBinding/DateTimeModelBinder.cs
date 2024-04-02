//using LinFx.Extensions.Timing;
//using Microsoft.AspNetCore.Mvc.ModelBinding;

//namespace LinFx.Extensions.AspNetCore.Mvc.ModelBinding;

//public class DateTimeModelBinder(IClock clock, DateTimeModelBinder dateTimeModelBinder) : IModelBinder
//{
//    private readonly IClock _clock = clock;
//    private readonly DateTimeModelBinder _dateTimeModelBinder = dateTimeModelBinder;

//    public async Task BindModelAsync(ModelBindingContext bindingContext)
//    {
//        await _dateTimeModelBinder.BindModelAsync(bindingContext);
//        if (bindingContext.Result.IsModelSet && bindingContext.Result.Model is DateTime dateTime)
//        {
//            bindingContext.Result = ModelBindingResult.Success(_clock.Normalize(dateTime));
//        }
//    }
//}
