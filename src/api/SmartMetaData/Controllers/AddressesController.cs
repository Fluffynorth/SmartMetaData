using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Services;
using SmartMetaData.Utils;

namespace SmartMetaData.Controllers;

[ApiController]
[Route("addresses/{address}")]
public class AddressesController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AddressesController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("tokens")]
    public async Task<IActionResult> GetAllTokens(
        [FromRoute, Required] string address,
        [FromQuery, Required] EthereumNetwork network)
    {
        var parsedContractAddress = Address.Create(address);
        if (parsedContractAddress.IsFailure)
            return BadRequest($"Invalid {nameof(address)}");

        var tokens = await _tokenService.GetTokensForAddress(parsedContractAddress.Value, network);

        return Ok(tokens);
    }
}
