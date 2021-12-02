using System;
using System.Collections.Generic;

namespace LinFx.Data;

[Serializable]
public class ConnectionStrings : Dictionary<string, string>
{
    public const string DefaultConnectionStringName = "Default";

    public string Default
    {
        get => this.GetOrDefault(DefaultConnectionStringName);
        set => this[DefaultConnectionStringName] = value;
    }
}
