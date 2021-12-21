using System.Numerics;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Models.Entities;

public class Token
{
    public Address ContractAddress { get; set; }
    public BigInteger TokenId { get; set; }
}
