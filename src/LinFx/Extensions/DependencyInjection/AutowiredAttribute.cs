using System;

namespace LinFx.Extensions.DependencyInjection;

/// <summary>
/// @Autowired
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AutowiredAttribute : Attribute
{
}
