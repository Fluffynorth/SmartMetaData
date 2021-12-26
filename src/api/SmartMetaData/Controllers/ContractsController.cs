using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Services;

namespace SmartMetaData.Controllers;

[ApiController]
[Route("chain/{chain}/contracts/{contractAddress}")]
public class ContractsController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public ContractsController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("tokens/{tokenId}/uri")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTokenUri(
        [FromRoute, Required] EthereumChain chain,
        [FromRoute, Required] Address contractAddress,
        [FromRoute, Required] BigInteger tokenId,
        [FromQuery, Required] TokenType tokenType)
    {
        var tokenUri = await _tokenService.GetTokenUri(chain, contractAddress, tokenId, tokenType);
        if (tokenUri.IsFailure)
            return BadRequest(tokenUri.Error);

        return Ok(tokenUri.Value.OriginalString);
    }
}
