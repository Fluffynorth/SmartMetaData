using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Services;
using SmartMetaData.Utils;

namespace SmartMetaData.Controllers;

[ApiController]
[Route("contracts/{contractAddress}")]
public class ContractsController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public ContractsController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("tokens/{tokenId}/uri")]
    public async Task<IActionResult> GetTokenUri(
        [FromRoute, Required] string contractAddress,
        [FromRoute, Required] string tokenId,
        [FromQuery, Required] EthereumNetwork network)
    {
        var parsedContractAddress = Address.Create(contractAddress);
        if (parsedContractAddress.IsFailure)
            return BadRequest($"Invalid {nameof(contractAddress)}");

        var parsedTokenId = ParseUtils.ParseBigInteger(tokenId);
        if (parsedTokenId.IsFailure)
            return BadRequest($"Invalid {nameof(tokenId)}");

        var tokenUri = await _tokenService.GetTokenUri(parsedContractAddress.Value, parsedTokenId.Value, network);
        if (tokenUri.IsFailure)
            return BadRequest(tokenUri.Error);

        return Ok(tokenUri.Value.OriginalString);
    }
}
