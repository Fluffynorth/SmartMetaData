using System.Numerics;
using CSharpFunctionalExtensions;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.Model;
using Nethereum.Hex.HexConvertors.Extensions;
using SmartMetaData.Extensions;

namespace SmartMetaData.Services.Rlp;

public class RlpDecoderService : IRlpDecoder
{
    private const int StandardParameterLength = 64;

    public Result<string> DecodeString(string encodedString)
    {
        var decoder = new FunctionCallDecoder();
        var parameter = new Parameter("string");
        try
        {
            return decoder.DecodeSimpleTypeOutput<string>(parameter, encodedString);
        }
        catch (Exception e)
        {
            return Result.Failure<string>("Failed to decode string: " + e.Message);
        }
    }

    public Result<BigInteger> DecodeBigInteger(string encodedString)
    {
        var decoder = new FunctionCallDecoder();
        var parameter = new Parameter("uint256");
        try
        {
            return decoder.DecodeSimpleTypeOutput<BigInteger>(parameter, encodedString);
        }
        catch (Exception e)
        {
            return Result.Failure<BigInteger>("Failed to decode BigInteger: " + e.Message);
        }
    }

    public string[] ExtractArray(string encodedString)
        => encodedString == null
            ? Array.Empty<string>()
            : encodedString.RemoveHexPrefix().SplitByCharactersCount(StandardParameterLength).ToArray();
}
