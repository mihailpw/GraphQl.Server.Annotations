using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQl.Server.Annotations
{
    public static class NonNullExtensions
    {
        public static NonNull<T> AsNonNull<T>(this T value)
        {
            return value;
        }

        public static IEnumerable<NonNull<T>> AsNonNullEach<T>(this IEnumerable<T> values)
        {
            return values.Select(AsNonNull);
        }

        public static NonNull<IEnumerable<NonNull<T>>> AsNonNullAndEach<T>(this IEnumerable<T> values)
        {
            return AsNonNull(AsNonNullEach(values));
        }
    }

    public interface INonNull
    {
        object ValueObject { get; }
    }

    public struct NonNull<T> : INonNull, IEquatable<NonNull<T>>
    {
        object INonNull.ValueObject => Value;

        public T Value { get; }


        public NonNull(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }


        public static implicit operator T(NonNull<T> nonNull)
        {
            return nonNull.Value;
        }

        public static implicit operator NonNull<T>(T value)
        {
            return new NonNull<T>(value);
        }

        public static bool operator ==(NonNull<T> nonNull1, NonNull<T> nonNull2)
        {
            return nonNull1.Equals(nonNull2);
        }

        public static bool operator !=(NonNull<T> nonNull1, NonNull<T> nonNull2)
        {
            return !nonNull1.Equals(nonNull2);
        }


        public override bool Equals(object other)
        {
            return other is NonNull<T> otherNonNull && Equals(otherNonNull);
        }

        public bool Equals(NonNull<T> other)
        {
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"NonNull({Value.ToString()})";
        }
    }
}