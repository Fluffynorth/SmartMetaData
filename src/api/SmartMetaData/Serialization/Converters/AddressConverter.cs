using System.Text.Json;
using System.Text.Json.Serialization;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Serialization.Converters;

public class AddressConverter : JsonConverter<Address>
{
    public override Address Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Found token {reader.TokenType} but expected token {JsonTokenType.String}");
        }

        var addressResult = Address.Create(reader.GetString());
        if (addressResult.IsFailure)
        {
            throw new JsonException(addressResult.Error);
        }

        return addressResult.Value;
    }

    public override void Write(Utf8JsonWriter writer, Address value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString());
    }
}
