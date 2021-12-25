using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartMetaData.Utils;

namespace SmartMetaData.Converters;

public class BigIntegerConverter : JsonConverter<BigInteger>
{
    public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number || reader.TokenType == JsonTokenType.String)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var rawValue = doc.RootElement.GetRawText();
            var parseResult = ParseUtils.ParseBigInteger(rawValue);
            if (parseResult.IsFailure)
                throw new JsonException(parseResult.Error);

            return parseResult.Value;
        }

        throw new JsonException($"Found token {reader.TokenType} but expected token {JsonTokenType.Number} or {JsonTokenType.String}");
    }

    public override void Write(Utf8JsonWriter writer, BigInteger value, JsonSerializerOptions options)
    {
        var str = value.ToString(NumberFormatInfo.InvariantInfo);
        writer.WriteStringValue(str);
    }
}
