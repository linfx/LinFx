using LinFx.Extensions.ObjectExtending;
using System;

namespace LinFx.Domain.Entities
{
    public abstract class AggregateRoot : BasicAggregateRoot, IHasExtraProperties
    {
        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

        public virtual string ConcurrencyStamp { get; set; }

        protected AggregateRoot()
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

    public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>, IHasExtraProperties
    {
        public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

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
}
