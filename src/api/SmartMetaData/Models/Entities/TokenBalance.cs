using System.Numerics;

namespace SmartMetaData.Models.Entities;

public class TokenBalance : BaseTokenInfo
{
    public BigInteger Amount { get; set; }
}
