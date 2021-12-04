using System.Numerics;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using SmartMetaData.Domain.Models.Enums;
using SmartMetaData.Domain.Models.ValueObjects;
using SmartMetaData.Infrastructure.Models.Functions;
using SmartMetaData.Infrastructure.Options;

namespace SmartMetaData.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly RpcOptions _rpcOptions;

    public TokenService(IOptions<RpcOptions> rpcOptions)
    {
        _rpcOptions = rpcOptions.Value;
    }

    public async Task<Result<Uri>> GetTokenUri(Address contractAddress, BigInteger tokenId, EthereumNetwork network)
    {
        var tokenUriMessage = new Erc721TokenUriFunction
        {
            TokenId = tokenId,
        };

        var rpcUrl = _rpcOptions.GetRpcUrl(network);
        var rpcClient = new RpcClient(rpcUrl);
        var web3 = new Web3(rpcClient);
        var tokenUriHandler = web3.Eth.GetContractQueryHandler<Erc721TokenUriFunction>();
        var tokenUri = await tokenUriHandler.QueryAsync<string>(contractAddress, tokenUriMessage);

        if (string.IsNullOrEmpty(tokenUri))
            return Result.Failure<Uri>("TokenUri is null or empty");

        if (!Uri.TryCreate(tokenUri, UriKind.Absolute, out var parsedTokenUri))
            return Result.Failure<Uri>("TokenUri is not parseable");

        return Result.Success(parsedTokenUri);
    }
}
