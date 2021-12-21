using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using SmartMetaData.Attributes;

namespace SmartMetaData.Models.Events;

[Event("Transfer"), EventTopic("0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef")]
public class Erc721TransferEvent : IEventDTO
{
    [Parameter("address", "_from", 1, true)]
    public virtual string From { get; set; }

    [Parameter("address", "_to", 2, true)]
    public virtual string To { get; set; }

    [Parameter("uint256", "_tokenId", 3, true)]
    public virtual BigInteger TokenId { get; set; }
}
