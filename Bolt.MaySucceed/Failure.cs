namespace Bolt.MaySucceed;

public record Failure(int StatusCode, 
    string Reason, 
    Error[]? Errors = null)
{
    public Failure(string reason, Error[] errors)
    : this(400, reason, errors)
    {
    }
    
    public Failure(Error[] errors)
        : this("Please check the error(s)", errors)
    {
    }
    
    public Failure(string reason) : this(500, reason, null)
    {
    }
    
    public Dictionary<string,object>? MetaData { get; init; }

    public static implicit operator Failure(Error error) => new(new []{error});
    public static implicit operator Failure(Error[] errors) => new(errors);
}