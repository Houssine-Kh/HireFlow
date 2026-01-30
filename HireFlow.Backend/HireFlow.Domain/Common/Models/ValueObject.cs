using System.Collections.Generic;
using System.Linq;

namespace HireFlow.Domain.Common
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject? left, ValueObject? right)
        {
            // If one is null and the other isn't, they aren't equal
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }

            // If both are null, return true. If left is not null, check equality.
            return ReferenceEquals(left, null) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject? left, ValueObject? right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
        
        // Optional: Overload operators for cleaner syntax
        public static bool operator ==(ValueObject one, ValueObject two) => EqualOperator(one, two);
        public static bool operator !=(ValueObject one, ValueObject two) => NotEqualOperator(one, two);
    }
}