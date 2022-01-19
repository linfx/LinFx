using System;
using System.Collections.Generic;

namespace LinFx.Extensions.ObjectExtending;

/// <summary>
/// 属性扩展字典
/// </summary>
[Serializable]
public class ExtraPropertyDictionary : Dictionary<string, object>
{
    public ExtraPropertyDictionary() { }

    public ExtraPropertyDictionary(IDictionary<string, object> dictionary)
        : base(dictionary)
    {
    }
}
