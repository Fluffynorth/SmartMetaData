using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Models.Enums;
using SmartMetaData.Services;
using SmartMetaData.Utils;

namespace SmartMetaData.Controllers;

[ApiController]
[Route("chain/{chain}/blocks")]
public class BlocksController : ControllerBase
{
    private readonly IBlockService _blockService;

    public BlocksController(IBlockService blockService)
    {
        _blockService = blockService;
    }

    [HttpGet("latest")]
    [ProducesResponseType(typeof(Block), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLatestBlock([FromRoute, Required] EthereumChain chain)
    {
        var latestBlock = await _blockService.GetLatestBlock(chain);
        return Ok(latestBlock);
    }

    [HttpGet("{blockNumber}")]
    [ProducesResponseType(typeof(Block), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlockByNumber(
        [FromRoute, Required] EthereumChain chain,
        [FromRoute, Required] string blockNumber)
    {
        var parsedBlockNumber = ParseUtils.ParseBigInteger(blockNumber);
        if (parsedBlockNumber.IsFailure)
            return BadRequest($"Invalid {nameof(blockNumber)}");

        var latestBlock = await _blockService.GetBlockByNumber(chain, parsedBlockNumber.Value);
        return Ok(latestBlock);
    }
}
