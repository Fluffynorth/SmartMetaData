using System.Numerics;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Mapping;

public interface IApplicationMapper
{
    BaseTokenInfo ToBaseTokenInfo(Address contractAddress, BigInteger tokenId, TokenType tokenType);
}
