namespace LinFx.Extensions.Auditing;

/// <summary>
/// 实体变化类型
/// </summary>
public enum EntityChangeType : byte
{
    Created = 0,
    Updated = 1,
    Deleted = 2
}
