using JetBrains.Annotations;
using LinFx.Extensions.Timing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LinFx.Extensions.EntityFrameworkCore.ValueConverters;

public class DateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public DateTimeValueConverter(IClock clock, [CanBeNull] ConverterMappingHints? mappingHints = default)
        : base(x => x.HasValue ? clock.Normalize(x.Value) : x, x => x.HasValue ? clock.Normalize(x.Value) : x, mappingHints)
    { }
}