using System.Diagnostics.CodeAnalysis;

namespace Bolt.MaySucceed;

public readonly struct MaySucceed
{
    public MaySucceed()
    {
        StatusCode = 200;
        IsSucceed = true;
    }

    public MaySucceed(Failure failure)
    {
        StatusCode = failure.StatusCode;
        IsSucceed = false;
        Failure = failure;
    }
    
    public int StatusCode { get; private init; }


    [MemberNotNullWhen(returnValue: false, nameof(Failure))]
    public bool IsSucceed { get; private init; }


    [MemberNotNullWhen(returnValue: true, nameof(Failure))]
    public bool IsFailed => !IsSucceed;

    public Failure? Failure { get; private init; }

    private static readonly MaySucceed Instance = new();
    public static MaySucceed Ok() => Instance;
    public static MaySucceed<T> Ok<T>(T value) => new(value);

    public static implicit operator MaySucceed(bool isSucceed) => isSucceed
        ? Instance
        : new(new Failure("Undefined server error"));

    public static implicit operator MaySucceed(Failure failure) => new(failure);

    public static implicit operator MaySucceed(Error error) => new(BuildFailure(new[] { error }));

    public static implicit operator MaySucceed(Error[] errors) => new(BuildFailure(errors));

    private static Failure BuildFailure(Error[] errors)
    {
        return new Failure(errors);
    }
}

public readonly struct MaySucceed<T>
{
    public MaySucceed(T value)
    {
        StatusCode = 200;
        IsSucceed = true;
        Value = value;
    }

    public MaySucceed(Failure failure)
    {
        StatusCode = failure.StatusCode;
        IsSucceed = false;
        Failure = failure;
    }
    
    public int StatusCode { get; private init; }

    [MemberNotNullWhen(returnValue: false, nameof(Failure))]
    [MemberNotNullWhen(returnValue: true, nameof(Value))]
    public bool IsSucceed { get; private init; }


    [MemberNotNullWhen(returnValue: true, nameof(Failure))]
    [MemberNotNullWhen(returnValue: false, nameof(Value))]
    public bool IsFailed => !IsSucceed;

    /// <summary>
    /// return true when value is not null
    /// </summary>
    public bool HasValue => Value is not null;
    
    public Failure? Failure { get; private init; }

    [MaybeNull] public T Value { get; init; }

    public static MaySucceed<T> Ok(T value) => new(value);

    public static implicit operator MaySucceed<T>(T value) => new(value);

    public static implicit operator MaySucceed<T>(Failure failure) => new(failure);

    public static implicit operator MaySucceed<T>(Error error) => new(BuildFailure(new[] { error }));

    public static implicit operator MaySucceed<T>(Error[] errors) => new(BuildFailure(errors));

    private static Failure BuildFailure(Error[] errors)
    {
        return new Failure("Please check the error(s)", errors);
    }
}