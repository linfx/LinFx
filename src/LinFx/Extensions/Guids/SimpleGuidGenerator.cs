﻿namespace LinFx.Extensions.Guids;

/// <summary>
/// Implements <see cref="IGuidGenerator"/> by using <see cref="Guid.NewGuid"/>.
/// </summary>
public class SimpleGuidGenerator : IGuidGenerator
{
    public static SimpleGuidGenerator Instance { get; } = new SimpleGuidGenerator();

    public virtual Guid Create() => Guid.NewGuid();
}