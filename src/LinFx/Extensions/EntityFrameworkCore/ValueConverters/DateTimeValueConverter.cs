using JetBrains.Annotations;
using LinFx.Extensions.Timing;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace LinFx.Extensions.EntityFrameworkCore.ValueConverters
{
    public class DateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
    {
        public DateTimeValueConverter(IClock clock, [CanBeNull] ConverterMappingHints mappingHints = null)
            : base(
                x => x.HasValue ? clock.Normalize(x.Value) : x,
                x => x.HasValue ? clock.Normalize(x.Value) : x, mappingHints)
        {
        }
    }
}