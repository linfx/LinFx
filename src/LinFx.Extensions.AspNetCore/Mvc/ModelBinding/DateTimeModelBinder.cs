using LinFx.Extensions.Timing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.AspNetCore.Mvc.ModelBinding;

public class DateTimeModelBinder : IModelBinder
{
    private readonly DateTimeModelBinder _dateTimeModelBinder;
    private readonly IClock _clock;

    public DateTimeModelBinder(IClock clock, DateTimeModelBinder dateTimeModelBinder)
    {
        _clock = clock;
        _dateTimeModelBinder = dateTimeModelBinder;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        await _dateTimeModelBinder.BindModelAsync(bindingContext);
        if (bindingContext.Result.IsModelSet && bindingContext.Result.Model is DateTime dateTime)
        {
            bindingContext.Result = ModelBindingResult.Success(_clock.Normalize(dateTime));
        }
    }
}
