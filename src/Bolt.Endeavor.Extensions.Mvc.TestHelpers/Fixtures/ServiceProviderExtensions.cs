using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.Core;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures;

public static class ServiceProviderExtensions
{
    public static TService? TryGetServiceOf<TService, TImpl>(this IServiceProvider sp)
    {
        return sp.GetServices<TService>()
                    .FirstOrDefault(x => x != null 
                                         && x.GetType() == typeof(TImpl));
    }

    public static TService GetFakeService<TService>(this IServiceProvider sp) where TService : class
    {
        var result = sp.GetRequiredService<TService>();

        if (result is not ICallRouterProvider) throw new Exception("Not a substituted instance");
        
        result.ClearSubstitute();
        result.ClearReceivedCalls();

        return result;
    }

    public static T? TryGetFakeServiceOf<T, TImpl>(this IServiceProvider sp) where T : class
    {
        var result = sp.TryGetServiceOf<T,TImpl>();

        if (result == null) return null;
        
        if (result is not ICallRouterProvider) throw new Exception("Not a substituted instance");
        
        result.ClearSubstitute();
        result.ClearReceivedCalls();
        return result;
    }
}