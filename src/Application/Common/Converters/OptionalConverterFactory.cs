using System.Text.Json;
using System.Text.Json.Serialization;
using PromobayBackend.Domain.Common;

namespace PromobayBackend.Application.Common.Converters;

public class OptionalConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType && 
               typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type genericType = typeToConvert.GetGenericArguments()[0];
        Type converterType = typeof(OptionalConverter<>).MakeGenericType(genericType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}
