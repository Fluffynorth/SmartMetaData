using CSharpFunctionalExtensions;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Services;

public interface ITokenService
{
    Task<IReadOnlyCollection<TokenBalance>> GetTokensForAddress(EthereumChain chain, Address address);

    Task<Result<Token>> GetToken(EthereumChain chain, BaseTokenInfo tokenInfo);
    Task<Result<Uri>> GetTokenUri(EthereumChain chain, BaseTokenInfo tokenInfo);
}
