using System.Diagnostics.CodeAnalysis;

namespace Bolt.MaySucceed;

public readonly struct MayBe<T>
{
    public MayBe(T? value)
    {
        IsNone = value is null;
        Value = value;
    }

    public MayBe()
    {
        IsNone = true;
    }
    
    public T? Value { get; init; }
    
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsNone { get; init; }

    public static implicit operator T?(MayBe<T> mayBe) => mayBe.Value;
    public static implicit operator MayBe<T>(T? value) => new(value);
}