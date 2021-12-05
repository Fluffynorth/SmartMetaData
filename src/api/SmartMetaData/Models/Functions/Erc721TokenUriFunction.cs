using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using SmartMetaData.Attributes;

namespace SmartMetaData.Models.Functions;

[Function("tokenURI", "string"), FunctionHash("0xc87b56dd")]
public class Erc721TokenUriFunction : Function
{
    [Parameter("uint256", "_tokenId", 1)]
    public BigInteger TokenId { get; set; }
}
