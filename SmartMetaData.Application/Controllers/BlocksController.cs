using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SmartMetaData.Application.Utils;
using SmartMetaData.Domain.Models.Enums;
using SmartMetaData.Infrastructure.Services;

namespace SmartMetaData.Application.Controllers;

[ApiController]
[Route("blocks")]
public class BlocksController : ControllerBase
{
    private readonly IBlockService _blockService;

    public BlocksController(IBlockService blockService)
    {
        _blockService = blockService;
    }

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestBlock([FromQuery, Required] EthereumNetwork network)
    {
        var latestBlock = await _blockService.GetLatestBlock(network);
        return Ok(latestBlock);
    }

    [HttpGet("{blockNumber}")]
    public async Task<IActionResult> GetBlockByNumber(
        [FromRoute, Required] string blockNumber,
        [FromQuery, Required] EthereumNetwork network)
    {
        var parsedBlockNumber = ParseUtils.ParseBigInteger(blockNumber);
        if (parsedBlockNumber.IsFailure)
            return BadRequest($"Invalid {nameof(blockNumber)}");

        var latestBlock = await _blockService.GetBlockByNumber(parsedBlockNumber.Value, network);
        return Ok(latestBlock);
    }
}
