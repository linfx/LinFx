﻿using LinFx.Extensions.Auditing;
using LinFx.Extensions.ObjectExtending;

namespace LinFx.Domain.Entities;

/// <summary>
/// 聚合根
/// </summary>
public abstract class AggregateRoot : BasicAggregateRoot, IHasExtraProperties
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

    /// <summary>
    /// 同步标记
    /// </summary>
    [DisableAuditing]
    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = [];
        //this.SetDefaultsForExtraProperties();
    }

    //public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //{
    //    return ExtensibleObjectValidator.GetValidationErrors(
    //        this,
    //        validationContext
    //    );
    //}
}

/// <summary>
/// 聚合根
/// </summary>
public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>, IHasExtraProperties
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

    /// <summary>
    /// 同步标记
    /// </summary>
    [DisableAuditing]
    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = new ExtraPropertyDictionary();
        //this.SetDefaultsForExtraProperties();
    }

    protected AggregateRoot(TKey id)
        : base(id)
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
        ExtraProperties = new ExtraPropertyDictionary();
        //this.SetDefaultsForExtraProperties();
    }

    //public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //{
    //    return ExtensibleObjectValidator.GetValidationErrors(
    //        this,
    //        validationContext
    //    );
    //}
}
