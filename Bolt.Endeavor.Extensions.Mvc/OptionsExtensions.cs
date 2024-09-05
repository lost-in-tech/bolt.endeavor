using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bolt.Endeavor.Extensions.Mvc;

internal static class OptionsExtensions
{
    internal static void ScanAndConfigure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        ScanAndConfigure(services, configuration, [Assembly.GetExecutingAssembly()]);
    }
    
    internal static void ScanAndConfigure(this IServiceCollection services, 
        IConfiguration configuration,
        Assembly[] assemblies)
    {
        // Get all types with BindFromConfigAttribute
        var configTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new
            {
                Type = t,
                Attribute = t.GetCustomAttribute<BindFromConfigAttribute>()
            })
            .Where(x => x.Attribute != null);

        foreach (var configType in configTypes)
        {
            var att = configType.Attribute;
            
            if(att == null) continue;
            
            var sectionName = att.SectionName;

            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = $"App:{configType.Type.Name}";
            }
            
            var section = configuration.GetSection(sectionName);

            if (!section.Exists())
            {
                if (!att.Optional)
                {
                    // Throw exception if the config is missing and Optional is false
                    throw new InvalidOperationException(
                        $"Configuration section '{sectionName}' is missing for type '{configType.Type.FullName}'.");
                }

                // If optional and missing, skip registration
                continue;
            }

            // Use reflection to call services.Configure<TOptions>(section)
            var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
                .GetMethod(nameof(OptionsConfigurationServiceCollectionExtensions.Configure), new[] { typeof(IServiceCollection), typeof(IConfiguration) });

            var genericConfigureMethod = configureMethod?.MakeGenericMethod(configType.Type);

            genericConfigureMethod?.Invoke(null, new object[] { services, section });
        }
    }
}