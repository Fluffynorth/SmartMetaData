using System.Numerics;
using CSharpFunctionalExtensions;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Services;

public interface ITokenService
{
    Task<IReadOnlyCollection<TokenBalance>> GetTokensForAddress(EthereumChain chain, Address address);
    Task<Result<Uri>> GetTokenUri(EthereumChain chain, Address contractAddress, BigInteger tokenId, TokenType tokenType);
}
