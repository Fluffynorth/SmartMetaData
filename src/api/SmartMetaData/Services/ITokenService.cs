using System.Numerics;
using CSharpFunctionalExtensions;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Services;

public interface ITokenService
{
    Task<Result<Uri>> GetTokenUri(Address contractAddress, BigInteger tokenId, EthereumNetwork network);
}
