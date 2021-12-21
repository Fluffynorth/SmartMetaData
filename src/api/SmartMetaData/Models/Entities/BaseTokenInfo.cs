using System.Numerics;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Models.Entities;

public class BaseTokenInfo
{
    public Address ContractAddress { get; set; }
    public BigInteger TokenId { get; set; }
}
