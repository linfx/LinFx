namespace LinFx.Extensions.ObjectExtending;

public interface IHasExtraProperties
{
    /// <summary>
    /// 属性扩展
    /// </summary>
    ExtraPropertyDictionary ExtraProperties { get; }
}
