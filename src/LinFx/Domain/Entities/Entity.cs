using System;
using System.Collections.Generic;

namespace LinFx.Domain.Entities;

/// <summary>
/// 实体
/// </summary>
[Serializable]
public abstract class Entity : IEntity
{
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Keys = {GetKeys().JoinAsString(", ")}";
    }

    public abstract object[] GetKeys();
}

/// <summary>
/// 实体
/// </summary>
/// <typeparam name="TKey"></typeparam>
[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    /// <inheritdoc/>
    public virtual TKey Id { get; set; }

    protected Entity() { }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public override object[] GetKeys()
    {
        return new object[] { Id };
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }
}
