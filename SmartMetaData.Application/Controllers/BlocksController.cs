using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
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
}
