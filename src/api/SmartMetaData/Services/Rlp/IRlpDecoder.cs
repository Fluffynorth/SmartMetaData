using System.Numerics;
using CSharpFunctionalExtensions;

namespace SmartMetaData.Services.Rlp;

public interface IRlpDecoder
{
    Result<string> DecodeString(string encodedString);
    Result<BigInteger> DecodeBigInteger(string encodedString);
    string[] ExtractArray(string encodedString);
}
