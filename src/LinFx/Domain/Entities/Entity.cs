using System;

namespace LinFx.Domain.Entities
{
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }

        /// <inheritdoc/>
        //public override bool Equals(object obj)
        //{
        //    if (obj == null || !(obj is Entity<TPrimaryKey>))
        //    {
        //        return false;
        //    }

        //    //Same instances must be considered as equal
        //    if (ReferenceEquals(this, obj))
        //    {
        //        return true;
        //    }

        //    //Transient objects are not considered as equal
        //    var other = (Entity<TPrimaryKey>)obj;
        //    //if (IsTransient() && other.IsTransient())
        //    //{
        //    //    return false;
        //    //}

        //    //Must have a IS-A relation of types or must be same type
        //    //var typeOfThis = GetType();
        //    //var typeOfOther = other.GetType();
        //    //if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
        //    //{
        //    //    return false;
        //    //}

        //    return Id.Equals(other.Id);
        //}

        ///// <inheritdoc/>
        //public override int GetHashCode()
        //{
        //    return Id.GetHashCode();
        //}

        ///// <inheritdoc/>
        //public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        //{
        //    if (Equals(left, null))
        //    {
        //        return Equals(right, null);
        //    }
        //    return left.Equals(right);
        //}

        ///// <inheritdoc/>
        //public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        //{
        //    return !(left == right);
        //}

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("[{0} {1}]", GetType().Name, Id);
        }
    }

    public class Entity : Entity<int>, IEntity
    {
        public override int Id { get; set; }
    }
}
