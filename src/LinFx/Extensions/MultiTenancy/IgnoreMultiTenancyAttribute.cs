using System;

namespace LinFx.Extensions.MultiTenancy;

[AttributeUsage(AttributeTargets.All)]
public class IgnoreMultiTenancyAttribute : Attribute
{
}
