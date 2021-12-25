using System.Numerics;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Models.Enums;
using SmartMetaData.Options;

namespace SmartMetaData.Services;

public class BlockService : IBlockService
{
    private readonly RpcOptions _rpcOptions;

    public BlockService(IOptions<RpcOptions> rpcOptions)
    {
        _rpcOptions = rpcOptions.Value;
    }

    public Task<Block> GetLatestBlock(EthereumChain chain)
        => GetBlockWithoutTransactions("latest", chain);

    public Task<Block> GetBlockByNumber(BigInteger blockNumber, EthereumChain chain)
        => GetBlockWithoutTransactions(blockNumber.ToHexBigInteger().HexValue, chain);

    private async Task<Block> GetBlockWithoutTransactions(string blockNumber, EthereumChain chain)
    {
        var rpcUrl = _rpcOptions.GetRpcUrl(chain);
        var rpcClient = new RpcClient(rpcUrl);
        var rpcRequest = new RpcRequest(1, "eth_getBlockByNumber", blockNumber, false);
        var block = await rpcClient.SendRequestAsync<Block>(rpcRequest);
        return block;
    }
}
