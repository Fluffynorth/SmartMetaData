using System.Numerics;
using CSharpFunctionalExtensions;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Services;

public interface ITokenService
{
    Task<IReadOnlyCollection<TokenBalance>> GetTokensForAddress(Address address, EthereumChain chain);
    Task<Result<Uri>> GetTokenUri(Address contractAddress, BigInteger tokenId, EthereumChain chain);
}
