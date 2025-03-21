namespace PromobayBackend.Domain.Common;

public readonly struct Optional<T>
{
    private readonly T? _value;
    private readonly bool _hasValue;

    private Optional(T? value, bool hasValue)
    {
        _value = value;
        _hasValue = hasValue;
    }

    public static Optional<T> Some(T value)
    {
        return new Optional<T>(value, true);
    }

    public static Optional<T> None() => new(default, false);

    public bool HasValue => _hasValue;

    public T Value => _hasValue ? _value! : throw new InvalidOperationException("Optional has no value");

    public T? ValueOrDefault => _hasValue ? _value : default;

    public Optional<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
        if (mapper == null) throw new ArgumentNullException(nameof(mapper));
        return _hasValue ? Optional<TResult>.Some(mapper(_value!)) : Optional<TResult>.None();
    }

    public T Reduce(T defaultValue)
    {
        if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));
        return _hasValue ? _value! : defaultValue;
    }

    public override bool Equals(object? obj)
    {
        return obj is Optional<T> other && 
               _hasValue == other._hasValue &&
               EqualityComparer<T?>.Default.Equals(_value, other._value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_hasValue, _value);
    }

    public static bool operator ==(Optional<T> left, Optional<T> right) => left.Equals(right);
    public static bool operator !=(Optional<T> left, Optional<T> right) => !(left == right);

    public static implicit operator Optional<T>(T? value) => 
        value is null ? None() : Some(value);

    public static explicit operator T(Optional<T> optional) => optional.Value;
} 