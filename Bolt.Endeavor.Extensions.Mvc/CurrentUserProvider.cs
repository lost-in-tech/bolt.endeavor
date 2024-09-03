using Bolt.Endeavor.Extensions.Bus;
using Microsoft.AspNetCore.Http;

namespace Bolt.Endeavor.Extensions.Mvc;

public interface ICurrentUserMetaDataProvider
{
    Dictionary<string, object> Get();
}

internal sealed class CurrentUserProvider(IHttpContextAccessor contextAccessor,
    IEnumerable<ICurrentUserMetaDataProvider> metaDataProviders) : ICurrentUserProvider
{
    public CurrentUser Get()
    {
        return new CurrentUser
        {
            IsAuthenticated = contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false,
            UserId =  contextAccessor.HttpContext?.User.Identity?.Name,
            MetaData = BuildMetaData()
        };
    }

    private Dictionary<string, object>? BuildMetaData()
    {
        Dictionary<string,object>? result = null;
        
        foreach (var provider in metaDataProviders)
        {
            var data = provider.Get();

            if (data.Count > 0)
            {
                result ??= new();

                foreach (var item in data)
                {
                    result[item.Key] = item.Value;
                }
            }
        }

        return result;
    }
}