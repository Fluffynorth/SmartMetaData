using System.Numerics;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using SmartMetaData.Extensions;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.Functions;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Options;
using SmartMetaData.Services.DataDownloaders;
using SmartMetaData.Services.EventProcessor;

namespace SmartMetaData.Services;

public class TokenService : ITokenService
{
    private static readonly IReadOnlyCollection<IEventProcessor> EventProcessors = new IEventProcessor[]
    {
        new Erc721EventProcessor(),
        new Erc1155EventProcessor(),
        new Erc1155BatchEventProcessor(),
    };
    private readonly RpcOptions _rpcOptions;
    private readonly IDataDownloaderFactory _dataDownloaderFactory;
    private readonly ITokenMetadataParser _metadataParser;

    public TokenService(IOptions<RpcOptions> rpcOptions, IDataDownloaderFactory dataDownloaderFactory, ITokenMetadataParser metadataParser)
    {
        _rpcOptions = rpcOptions.Value;
        _dataDownloaderFactory = dataDownloaderFactory;
        _metadataParser = metadataParser;
    }

    public async Task<IReadOnlyCollection<TokenBalance>> GetTokensForAddress(EthereumChain chain, Address address)
    {
        var rpcUrl = _rpcOptions.GetRpcUrl(chain);
        var rpcClient = new RpcClient(rpcUrl);
        var web3 = new Web3(rpcClient);

        var tasks = EventProcessors.SelectMany(x => new[]
        {
            x.GetTokenTransfers(web3.Client, fromAddress: null, toAddress: address), // incoming tokens
            x.GetTokenTransfers(web3.Client, fromAddress: address, toAddress: null), // outgoing tokens
        }).ToArray();
        await Task.WhenAll(tasks);

        var eventLogs = tasks.SelectMany(x => x.Result)
            .OrderBy(x => new BigInteger(x.Log.BlockNumber))
            .ThenBy(x => new BigInteger(x.Log.TransactionIndex))
            .ThenBy(x => new BigInteger(x.Log.LogIndex))
            .ToArray();

        var calculator = new TokensBalanceCalculator(address);
        foreach (var eventLog in eventLogs)
        {
            var tokenTransfer = eventLog.Event;
            calculator.Apply(tokenTransfer);
        }

        return calculator.GetBalance();
    }

    public async Task<Result<Token>> GetToken(EthereumChain chain, BaseTokenInfo tokenInfo)
    {
        var tokenUriResult = await GetTokenUri(chain, tokenInfo);
        if (tokenUriResult.IsFailure)
            return Result.Failure<Token>(tokenUriResult.Error);

        var tokenUri = tokenUriResult.Value;
        var tokenMetadataResult = await GetTokenMetadata(tokenUri);
        if (tokenMetadataResult.IsFailure)
            return Result.Failure<Token>(tokenMetadataResult.Error);

        var tokenMetadata = tokenMetadataResult.Value;
        var token = new Token
        {
            ContractAddress = tokenInfo.ContractAddress,
            TokenId = tokenInfo.TokenId,
            Type = tokenInfo.Type,
            Metadata = tokenMetadata,
        };

        return token;
    }

    public async Task<Result<Uri>> GetTokenUri(EthereumChain chain, BaseTokenInfo tokenInfo)
    {
        var rpcUrl = _rpcOptions.GetRpcUrl(chain);
        var rpcClient = new RpcClient(rpcUrl);
        var web3 = new Web3(rpcClient);

        var task = tokenInfo.Type switch
        {
            TokenType.Erc721 => web3.SafeQuery(tokenInfo.ContractAddress, new Erc721TokenUriFunction {TokenId = tokenInfo.TokenId}),
            TokenType.Erc1155 => web3.SafeQuery(tokenInfo.ContractAddress, new Erc1155UriFunction {Id = tokenInfo.TokenId}),
            _ => throw new ArgumentOutOfRangeException(nameof(tokenInfo.Type), tokenInfo.Type, null)
        };
        var result = await task;
        if (result.IsFailure)
            return Result.Failure<Uri>($"Contract call failed: {result.Error}");

        var tokenUri = result.Value;
        if (string.IsNullOrEmpty(tokenUri))
            return Result.Failure<Uri>("TokenUri is null or empty");

        tokenUri = FillTemplateUri(tokenUri, tokenInfo.TokenId);
        tokenUri = SetIpfsGateway(tokenUri);

        if (!Uri.TryCreate(tokenUri, UriKind.Absolute, out var parsedTokenUri))
            return Result.Failure<Uri>("TokenUri is not parseable");

        return Result.Success(parsedTokenUri);
    }

    public async Task<Result<NftTokenMetadata>> GetTokenMetadata(Uri tokenUri)
    {
        if (tokenUri == null)
            return Result.Failure<NftTokenMetadata>("Token URI is null or empty");

        var dataDownloaderResult = _dataDownloaderFactory.Create(tokenUri.Scheme);
        if (dataDownloaderResult.IsFailure)
            return Result.Failure<NftTokenMetadata>(dataDownloaderResult.Error);

        var dataDownloader = dataDownloaderResult.Value;
        var rawTokenMetadataResult = await dataDownloader.GetString(tokenUri.OriginalString);
        if (rawTokenMetadataResult.IsFailure)
            return Result.Failure<NftTokenMetadata>(rawTokenMetadataResult.Error);

        var rawTokenMetadata = rawTokenMetadataResult.Value;
        var parsedTokenMetadataResult = _metadataParser.Parse(rawTokenMetadata);
        if (parsedTokenMetadataResult.IsFailure)
            return Result.Failure<NftTokenMetadata>(parsedTokenMetadataResult.Error);

        return parsedTokenMetadataResult.Value;
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
}
