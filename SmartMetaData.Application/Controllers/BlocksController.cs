using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using SmartMetaData.Domain.Models.Enums;
using SmartMetaData.Infrastructure.Options;

namespace SmartMetaData.Application.Controllers;

[ApiController]
[Route("blocks")]
public class BlocksController : ControllerBase
{
    private readonly RpcOptions _rpcOptions;

    public BlocksController(IOptions<RpcOptions> options)
    {
        _rpcOptions = options.Value;
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestBlock([FromQuery, Required] EthereumNetwork network)
    {
        var rpcUrl = _rpcOptions.GetRpcUrl(network);
        if (rpcUrl == null)
            return BadRequest("Invalid network");

        var rpcClient = new RpcClient(rpcUrl);
        var web3 = new Web3(rpcClient);
        var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(BlockParameter.CreateLatest());
        return Ok(block);
    }
}
