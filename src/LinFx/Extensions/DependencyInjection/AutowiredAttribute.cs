using System;

namespace LinFx
{
    /// <summary>
    /// @Autowired
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AutowiredAttribute : Attribute
    {
    }
}
