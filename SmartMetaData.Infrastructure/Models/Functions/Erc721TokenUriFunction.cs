using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using SmartMetaData.Infrastructure.Attributes;

namespace SmartMetaData.Infrastructure.Models.Functions;

[Function("tokenURI"), FunctionHash("0xc87b56dd")]
public class Erc721TokenUriFunction : Function
{
    [Parameter("uint256", "_tokenId", 1)]
    public BigInteger TokenId { get; set; }
}