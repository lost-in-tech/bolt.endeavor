using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers;

public static class JsonSerializerOptionsFactory
{
    private static readonly Lazy<JsonSerializerOptions> Instance = new(() =>
    {
        var opt = new JsonSerializerOptions();
        opt.Converters.Add(new JsonStringEnumConverter());
        opt.Converters.Add(new JsonTypeConverter());
        opt.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opt.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        opt.WriteIndented = true;
        opt.PropertyNameCaseInsensitive = true;

        return opt;
    });
    
    public static JsonSerializerOptions Create() => Instance.Value;
}