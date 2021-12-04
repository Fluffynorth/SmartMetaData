using System.Numerics;
using CSharpFunctionalExtensions;
using Nethereum.Hex.HexConvertors.Extensions;

namespace SmartMetaData.Application.Utils;

public static class ParseUtils
{
    public static Result<BigInteger> ParseBigInteger(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<BigInteger>("Value is null or empty");
        }

        if (value.HasHexPrefix() && value.IsHex())
        {
            return Result.Success(value.HexToBigInteger(false));
        }

        if (!BigInteger.TryParse(value, out var parsedBigInteger))
        {
            return Result.Failure<BigInteger>("Invalid value");
        }

        return Result.Success(parsedBigInteger);
    }
}
