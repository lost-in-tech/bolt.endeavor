using System.Text.Json;
using System.Text.Json.Serialization;
using Shouldly;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers;

public static class ShouldlyExtensions
{
    public static void ShouldMatchContent<T>(this T source, 
        string? msg = null, 
        string? discriminator = null)
    {
        var serialized = JsonSerializer.Serialize(source, JsonSerializerOptionsFactory.Create());
        serialized.ShouldMatchApproved(b =>
        {
            b.SubFolder("approved");
            b.UseCallerLocation();
            if (!string.IsNullOrWhiteSpace(discriminator))
            {
                b.WithDiscriminator(discriminator);
            }
        }, msg);
    }
}