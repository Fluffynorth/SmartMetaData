using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Services;

namespace SmartMetaData.Controllers;

[ApiController]
[Route("chain/{chain}/addresses/{address}")]
public class AddressesController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AddressesController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("tokens")]
    [ProducesResponseType(typeof(IReadOnlyCollection<TokenBalance>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTokens(
        [FromRoute, Required] EthereumChain chain,
        [FromRoute, Required] Address address)
    {
        var tokens = await _tokenService.GetTokensForAddress(chain, address);
        return Ok(tokens);
    }
}
