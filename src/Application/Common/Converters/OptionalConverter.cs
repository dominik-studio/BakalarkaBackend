using System.Text.Json;
using System.Text.Json.Serialization;
using PromobayBackend.Domain.Common;

namespace PromobayBackend.Application.Common.Converters;

public class OptionalConverter<T> : JsonConverter<Optional<T>>
{
    public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return Optional<T>.None();
        
        var value = JsonSerializer.Deserialize<T>(ref reader, options);
        return Optional<T>.Some(value);
    }

    public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            JsonSerializer.Serialize(writer, value.Value, options);
        else
            writer.WriteNullValue();
    }
} 
