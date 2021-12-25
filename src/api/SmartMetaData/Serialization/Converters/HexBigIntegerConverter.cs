using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Nethereum.Hex.HexTypes;

namespace SmartMetaData.Serialization.Converters;

public class HexBigIntegerConverter : JsonConverter<HexBigInteger>
{
    public override HexBigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"Found token {reader.TokenType} but expected token {JsonTokenType.String}");

        using var doc = JsonDocument.ParseValue(ref reader);
        return BigInteger.Parse(doc.RootElement.GetRawText(), NumberFormatInfo.InvariantInfo).ToHexBigInteger();
    }

    public override void Write(Utf8JsonWriter writer, HexBigInteger value, JsonSerializerOptions options)
    {
        var str = value.ToString();
        using var doc = JsonDocument.Parse(str);
        doc.WriteTo(writer);
    }
}
