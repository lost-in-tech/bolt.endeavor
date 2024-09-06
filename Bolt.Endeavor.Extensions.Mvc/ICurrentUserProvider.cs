namespace Bolt.Endeavor.Extensions.Mvc;

public interface ICurrentUserProvider
{
    CurrentUser Get();
}

public record CurrentUser
{
    public bool IsAuthenticated { get; init; }
    public string? UserId { get; init; }
    public Dictionary<string,object>? MetaData { get; init; }
}