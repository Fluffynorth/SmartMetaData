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
        => GetBlockWithoutTransactions(chain, "latest");

    public Task<Block> GetBlockByNumber(EthereumChain chain, BigInteger blockNumber)
        => GetBlockWithoutTransactions(chain, blockNumber.ToHexBigInteger().HexValue);

    private async Task<Block> GetBlockWithoutTransactions(EthereumChain chain, string blockNumber)
    {
        var rpcUrl = _rpcOptions.GetRpcUrl(chain);
        var rpcClient = new RpcClient(rpcUrl);
        var rpcRequest = new RpcRequest(1, "eth_getBlockByNumber", blockNumber, false);
        var block = await rpcClient.SendRequestAsync<Block>(rpcRequest);
        return block;
    }
}
