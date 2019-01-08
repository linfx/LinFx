using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LinFx.Domain.Values
{
    /// <summary>
    /// 值对象
    /// </summary>
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            ValueObject other = (ValueObject)obj;
            var thisValues = GetAtomicValues().GetEnumerator();
            var otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }
                if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }
            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
             .Select(x => x != null ? x.GetHashCode() : 0)
             .Aggregate((x, y) => x ^ y);
        }

        public ValueObject GetCopy()
        {
            return MemberwiseClone() as ValueObject;
        }
    }

    //Inspired from https://blogs.msdn.microsoft.com/cesardelatorre/2011/06/06/implementing-a-value-object-base-class-supertype-patternddd-patterns-related/
    /// <summary>
    /// Base class for value objects.
    /// </summary>
    /// <typeparam name="TValueObject">The type of the value object.</typeparam>
    public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {
        public bool Equals(TValueObject other)
        {
            if ((object)other == null)
            {
                return false;
            }

            var publicProperties = GetPropertiesForCompare();
            if (!publicProperties.Any())
            {
                return true;
            }

            return publicProperties.All(property => Equals(property.GetValue(this, null), property.GetValue(other, null)));
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var item = obj as ValueObject<TValueObject>;
            return (object)item != null && Equals((TValueObject)item);

        }

        public override int GetHashCode()
        {
            const int index = 1;
            const int initialHasCode = 31;

            var publicProperties = GetPropertiesForCompare();

            if (!publicProperties.Any())
            {
                return initialHasCode;
            }

            var hashCode = initialHasCode;
            var changeMultiplier = false;

            foreach (var property in publicProperties)
            {
                var value = property.GetValue(this, null);

                if (value == null)
                {
                    //support {"a",null,null,"a"} != {null,"a","a",null}
                    hashCode = hashCode ^ (index * 13);
                    continue;
                }

                hashCode = hashCode * (changeMultiplier ? 59 : 114) + value.GetHashCode();
                changeMultiplier = !changeMultiplier;
            }

            return hashCode;
        }

        public static bool operator ==(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if ((x is null) || (y is null))
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(ValueObject<TValueObject> x, ValueObject<TValueObject> y)
        {
            return !(x == y);
        }

        private PropertyInfo[] GetPropertiesForCompare()
        {
            //return GetType().GetTypeInfo().GetProperties().Where(t => ReflectionHelper.GetSingleAttributeOrDefault<IgnoreOnCompareAttribute>(t) == null).ToArray();
            throw new NotImplementedException();
        }
    }
}
