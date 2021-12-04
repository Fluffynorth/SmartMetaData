using System.Numerics;
using CSharpFunctionalExtensions;
using SmartMetaData.Domain.Models.Enums;
using SmartMetaData.Domain.Models.ValueObjects;

namespace SmartMetaData.Infrastructure.Services;

public interface ITokenService
{
    Task<Result<Uri>> GetTokenUri(Address contractAddress, BigInteger tokenId, EthereumNetwork network);
}
