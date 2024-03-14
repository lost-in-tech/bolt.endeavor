namespace Bolt.Endeavor;

public record Error(string Message, 
    string? PropertyName = null, 
    string? Code = null)
{
    public static implicit operator Task<MaySucceed>(Error error) => Task.FromResult<MaySucceed>(error);
}