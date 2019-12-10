namespace LinFx.Domain.Models
{
    /// <summary>
    /// 领域实体
    /// </summary>
    public abstract class Entity : IEntity
    {
        public abstract object[] GetKeys();
    }

    /// <summary>
    /// 领域实体
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    public abstract class Entity<Tkey> : Entity, IEntity<Tkey>
    {
        private int? _requestedHashCode;
        private Tkey _Id;

        public virtual Tkey Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public bool IsTransient()
        {
            return Id == null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<Tkey>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (Entity<Tkey>)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity<Tkey> left, Entity<Tkey> right)
        {
            if (Equals(left, null))
                return Equals(right, null);
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<Tkey> left, Entity<Tkey> right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }

        public override object[] GetKeys()
        {
            return new object[] { Id };
        }
    }
}
