namespace LinFx.Extensions.ObjectExtending
{
    /// <summary>
    /// 属性扩展
    /// </summary>
    public interface IHasExtraProperties
    {
        ExtraPropertyDictionary ExtraProperties { get; }
    }
}
