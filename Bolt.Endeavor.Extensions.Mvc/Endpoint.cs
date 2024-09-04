using Microsoft.AspNetCore.Routing;

namespace Bolt.Endeavor.Extensions.Mvc;

public interface IEndpoint
{
    string? GroupName { get; }
    void Configure(IEndpointRouteBuilder builder);
}

public abstract class Endpoint : IEndpoint
{
    string? IEndpoint.GroupName => null;
    public abstract void Configure(IEndpointRouteBuilder builder);
}

public abstract class Endpoint<TGroup> : IEndpoint
{
    string? IEndpoint.GroupName => typeof(TGroup).AssemblyQualifiedName;
    public abstract void Configure(IEndpointRouteBuilder builder);
}

public interface IGroupEndpoint
{
    RouteGroupBuilder Get(IEndpointRouteBuilder builder);
}