using Microsoft.Extensions.Options;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Domain.Models.Enums;
using SmartMetaData.Infrastructure.Options;

namespace SmartMetaData.Infrastructure.Services;

public class BlockService : IBlockService
{
    private readonly RpcOptions _rpcOptions;

    public BlockService(IOptions<RpcOptions> rpcOptions)
    {
        _rpcOptions = rpcOptions.Value;
    }

    public async Task<Block> GetLatestBlock(EthereumNetwork network)
    {
        var rpcUrl = _rpcOptions.GetRpcUrl(network);
        var rpcClient = new RpcClient(rpcUrl);
        var rpcRequest = new RpcRequest(1, "eth_getBlockByNumber", "latest", false);
        var block = await rpcClient.SendRequestAsync<Block>(rpcRequest);
        return block;
    }
}
