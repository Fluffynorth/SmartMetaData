using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
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
    public async Task<IActionResult> GetLatestBlock()
    {
        var web3 = new Web3(_rpcOptions.Url!.ToString());
        var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        return Ok(blockNumber.ToString());
    }
}
