using System.Diagnostics.CodeAnalysis;

namespace Bolt.MaySucceed;

public readonly struct MayBe<T>
{
    public MayBe(T? value)
    {
        IsNone = value is null;
        HasValue = !IsNone;
        Value = value;
    }

    public MayBe()
    {
        IsNone = true;
        HasValue = !IsNone;
    }
    
    public T? Value { get; }
    
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsNone { get; }
    
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue { get; }

    public static implicit operator T?(MayBe<T> mayBe) => mayBe.Value;
    public static implicit operator MayBe<T>(T? value) => new(value);
}