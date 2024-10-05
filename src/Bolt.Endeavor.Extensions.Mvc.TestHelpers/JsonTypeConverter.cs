using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bolt.Endeavor.Extensions.Mvc.TestHelpers;

internal sealed class JsonTypeConverter : JsonConverter<Type>
{
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? typeName = reader.GetString();

        if (string.IsNullOrWhiteSpace(typeName))
        {
            return null!;
        }

        // Attempt to get the Type from the type name
        Type? type = Type.GetType(typeName);

        if (type == null)
        {
            throw new JsonException($"Unable to find type: {typeName}");
        }

        return type;
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.AssemblyQualifiedName);
    }
}