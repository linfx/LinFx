namespace LinFx.Extensions.Auditing;

/// <summary>
/// Standard interface for an entity that MAY have a creator.
/// </summary>
public interface IMayHaveCreator
{
    /// <summary>
    /// Id of the creator.
    /// </summary>
    string? CreatorId { get; set; }
}

public interface IMayHaveCreator<TCreator>
{
    /// <summary>
    /// Reference to the creator.
    /// </summary>
    TCreator Creator { get; set; }
}
