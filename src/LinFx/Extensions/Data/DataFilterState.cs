namespace LinFx.Extensions.Data;

/// <summary>
/// 数据过滤状态
/// </summary>
public class DataFilterState
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }

    public DataFilterState(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }

    /// <summary>
    /// 克隆
    /// </summary>
    /// <returns></returns>
    public DataFilterState Clone()
    {
        return new DataFilterState(IsEnabled);
    }
}
