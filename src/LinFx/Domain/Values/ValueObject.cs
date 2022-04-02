using System.Collections.Generic;

namespace LinFx.Domain.Values;

/// <summary>
/// 值对象
/// 很多对象没有概念上的表示，他们描述了一个事务的某种特征。
/// 用于描述领域的某个方面而本身没有概念表示的对象称为Value Object（值对象）。
/// </summary>
public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetAtomicValues();

    public bool ValueEquals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        ValueObject other = (ValueObject)obj;

        IEnumerator<object> thisValues = GetAtomicValues().GetEnumerator();
        IEnumerator<object> otherValues = other.GetAtomicValues().GetEnumerator();

        while (thisValues.MoveNext() && otherValues.MoveNext())
        {
            if (thisValues.Current is null ^ otherValues.Current is null)
                return false;

            if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                return false;
        }

        return !thisValues.MoveNext() && !otherValues.MoveNext();
    }
}
