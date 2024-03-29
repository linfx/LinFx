namespace LinFx.Extensions.Data;

public static class CommonDbProperties
{
    /// <summary>
    /// This table prefix is shared by most of the modules.
    /// You can change it to set table prefix for all modules using this.
    /// 
    /// Default value: "".
    /// </summary>
    public static string DbTablePrefix { get; set; } = string.Empty;

    /// <summary>
    /// Default value: null.
    /// </summary>
    public static string? DbSchema { get; set; } = null;
}
