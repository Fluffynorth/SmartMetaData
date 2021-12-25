using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Services;
using SmartMetaData.Utils;

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
        [FromRoute, Required] string contractAddress,
        [FromRoute, Required] string tokenId)
    {
        var parsedContractAddress = Address.Create(contractAddress);
        if (parsedContractAddress.IsFailure)
            return BadRequest($"Invalid {nameof(contractAddress)}");

        var parsedTokenId = ParseUtils.ParseBigInteger(tokenId);
        if (parsedTokenId.IsFailure)
            return BadRequest($"Invalid {nameof(tokenId)}");

        var tokenUri = await _tokenService.GetTokenUri(chain, parsedContractAddress.Value, parsedTokenId.Value);
        if (tokenUri.IsFailure)
            return BadRequest(tokenUri.Error);

        return Ok(tokenUri.Value.OriginalString);
    }
}
