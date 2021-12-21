using System.Numerics;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using SmartMetaData.Constants;
using SmartMetaData.Extensions;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.Functions;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Options;

namespace SmartMetaData.Services;

public class TokenService : ITokenService
{
    private readonly RpcOptions _rpcOptions;

    public TokenService(IOptions<RpcOptions> rpcOptions)
    {
        _rpcOptions = rpcOptions.Value;
    }

    public async Task<List<Token>> GetTokensForAddress(Address address, EthereumNetwork network)
    {
        var rpcUrl = _rpcOptions.GetRpcUrl(network);
        var rpcClient = new RpcClient(rpcUrl);
        var web3 = new Web3(rpcClient);

        var incomingTokenLogs = await GetTokenTransferLogs(web3, TokenType.Erc721, fromAddress: null, address);
        var outgoingTokenLogs = await GetTokenTransferLogs(web3, TokenType.Erc721, fromAddress: address, null);

        return new List<Token>();
    }

    public async Task<Result<Uri>> GetTokenUri(Address contractAddress, BigInteger tokenId, EthereumNetwork network)
    {
        var rpcUrl = _rpcOptions.GetRpcUrl(network);
        var rpcClient = new RpcClient(rpcUrl);
        var web3 = new Web3(rpcClient);

        var erc721Function = new Erc721TokenUriFunction { TokenId = tokenId };
        var erc1155Function = new Erc1155UriFunction { Id = tokenId };

        var erc721Task = web3.SafeQuery(contractAddress, erc721Function);
        var erc1155Task = web3.SafeQuery(contractAddress, erc1155Function);

        await Task.WhenAll(erc721Task, erc1155Task);

        var values = new[] { erc721Task.Result, erc1155Task.Result };
        if (values.All(x => x.IsFailure))
            return Result.Failure<Uri>("Contract calls failed");

        var tokenUri = values.First(x => x.IsSuccess).Value;
        if (string.IsNullOrEmpty(tokenUri))
            return Result.Failure<Uri>("TokenUri is null or empty");

        tokenUri = FillTemplateUri(tokenUri, tokenId);
        tokenUri = SetIpfsGateway(tokenUri);

        if (!Uri.TryCreate(tokenUri, UriKind.Absolute, out var parsedTokenUri))
            return Result.Failure<Uri>("TokenUri is not parseable");

        return Result.Success(parsedTokenUri);
    }

    private static string FillTemplateUri(string tokenUri, BigInteger tokenId)
    {
        const string idTemplate = "{id}";

        if (!tokenUri.Contains(idTemplate))
            return tokenUri;

        var tokenIdInHex = tokenId.ToHexBigInteger().HexValue.RemoveHexPrefix().PadLeft(64, '0');
        return tokenUri.Replace(idTemplate, tokenIdInHex);
    }

    private string SetIpfsGateway(string tokenUri)
    {
        const string ipfsPrefix = "ipfs://";
        const string ipfsGateway = "https://ipfs.infura.io/ipfs/";

        if (!tokenUri.StartsWith(ipfsPrefix, StringComparison.InvariantCultureIgnoreCase))
            return tokenUri;

        return tokenUri.Replace(ipfsPrefix, ipfsGateway);
    }

    private static async Task<FilterLog[]> GetTokenTransferLogs(Web3 web3, TokenType tokenType, Address fromAddress, Address toAddress)
    {
        var eventTopic = tokenType == TokenType.Erc721 ? TopicConstants.Erc721Transfer : TopicConstants.Erc1155Transfer;

        var logs = await web3.Client.SendRequestAsync<FilterLog[]>(new RpcRequest(1, "eth_getLogs", new
        {
            fromBlock = BigInteger.Zero.ToHexBigInteger().HexValue,
            toBlock = "latest",
            topics = new string[]
            {
                eventTopic,
                fromAddress?.ToLongFormatString(),
                toAddress?.ToLongFormatString(),
            },
        }));

        return logs.Where(x => !x.Removed).ToArray();
    }
}
