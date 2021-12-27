using System.Numerics;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Mapping;

public class ApplicationMapper : IApplicationMapper
{
    public BaseTokenInfo ToBaseTokenInfo(Address contractAddress, BigInteger tokenId, TokenType tokenType)
        => new BaseTokenInfo
        {
            ContractAddress = contractAddress,
            TokenId = tokenId,
            Type = tokenType,
        };
}
