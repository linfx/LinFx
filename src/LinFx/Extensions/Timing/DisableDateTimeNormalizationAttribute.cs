using System;

namespace LinFx.Extensions.Timing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
public class DisableDateTimeNormalizationAttribute : Attribute
{
}
