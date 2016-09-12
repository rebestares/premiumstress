using System;

namespace Premiumstress.Core
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Type GetUnproxiedType()
        {
            var t = GetType();
            if (t.AssemblyQualifiedName.StartsWith("System.Data.Entity."))
            {
                // it's a proxied type
                t = t.BaseType;
            }
            return t;
        }

        /// <summary>
        ///     Transient objects are not associated with an item already in storage.  For instance,
        ///     a Product entity is transient if its Id is 0.
        /// </summary>
        public virtual bool IsTransientRecord()
        {
            return Id == 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        protected virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (HasSameNonDefaultIds(other))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.Equals(otherType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (IsTransientRecord())
            {
                return base.GetHashCode();
            }
            unchecked
            {
                // It's possible for two objects to return the same hash code based on
                // identically valued properties, even if they're of two different types,
                // so we include the object's type in the hash calculation
                var hashCode = GetUnproxiedType().GetHashCode();
                return (hashCode*31) ^ Id.GetHashCode();
            }
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }

        private bool HasSameNonDefaultIds(BaseEntity other)
        {
            return !IsTransientRecord() && !other.IsTransientRecord() && Id == other.Id;
        }
    }
}