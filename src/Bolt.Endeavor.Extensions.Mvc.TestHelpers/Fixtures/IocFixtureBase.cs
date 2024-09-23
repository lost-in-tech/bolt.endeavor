using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers.Fixtures;

public abstract class IocFixtureBase
{
    private readonly Lazy<IServiceProvider> _sp;

    protected IocFixtureBase()
    {
        _sp = new Lazy<IServiceProvider>(BuildServiceProvider);
    }

    private IServiceProvider BuildServiceProvider()
    {
        var sc = new ServiceCollection();
        var configBuilder = new ConfigurationBuilder();
        SetupConfiguration(configBuilder);
        ConfigureServices(sc, configBuilder.Build());
        return sc.BuildServiceProvider();
    }
    
    protected virtual void SetupConfiguration(IConfigurationBuilder builder)
    {}
    
    protected abstract void ConfigureServices(IServiceCollection collection, IConfiguration configuration);

    public IServiceScope CreateScope() => _sp.Value.CreateScope();

    public async Task WithScope(Func<IServiceProvider,Task> action)
    {
        using var scope = CreateScope();
        await action.Invoke(scope.ServiceProvider);
    }
    
    public void WithScope(Action<IServiceProvider> action)
    {
        using var scope = CreateScope();
        action.Invoke(scope.ServiceProvider);
    }
}