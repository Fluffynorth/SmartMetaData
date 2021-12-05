using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SmartMetaData.Application.Utils;
using SmartMetaData.Domain.Models.Enums;
using SmartMetaData.Domain.Models.ValueObjects;
using SmartMetaData.Infrastructure.Services;

namespace SmartMetaData.Application.Controllers;

[ApiController]
[Route("tokens")]
public class TokensController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokensController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("{contractAddress}/{tokenId}/uri")]
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
