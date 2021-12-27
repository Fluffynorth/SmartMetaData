using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using SmartMetaData.Mapping;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Services;

namespace SmartMetaData.Controllers;

[ApiController]
[Route("chain/{chain}/contracts/{contractAddress}")]
public class ContractsController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IApplicationMapper _mapper;

    public ContractsController(ITokenService tokenService, IApplicationMapper mapper)
    {
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpGet("tokens/{tokenId}")]
    [ProducesResponseType(typeof(Token), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetToken(
        [FromRoute, Required] EthereumChain chain,
        [FromRoute, Required] Address contractAddress,
        [FromRoute, Required] BigInteger tokenId,
        [FromQuery, Required] TokenType tokenType)
    {
        var baseTokenInfo = _mapper.ToBaseTokenInfo(contractAddress, tokenId, tokenType);
        var tokenResult = await _tokenService.GetToken(chain, baseTokenInfo);
        if (tokenResult.IsFailure)
            return BadRequest(tokenResult.Error);

        var token = tokenResult.Value;
        return Ok(token);
    }

    [HttpGet("tokens/{tokenId}/uri")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTokenUri(
        [FromRoute, Required] EthereumChain chain,
        [FromRoute, Required] Address contractAddress,
        [FromRoute, Required] BigInteger tokenId,
        [FromQuery, Required] TokenType tokenType)
    {
        var baseTokenInfo = _mapper.ToBaseTokenInfo(contractAddress, tokenId, tokenType);
        var tokenUriResult = await _tokenService.GetTokenUri(chain, baseTokenInfo);
        if (tokenUriResult.IsFailure)
            return BadRequest(tokenUriResult.Error);

        var tokenUri = tokenUriResult.Value;
        return Ok(tokenUri.OriginalString);
    }
}
