using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using SmartMetaData.Attributes;

namespace SmartMetaData.Models.Events;

[Event("TransferSingle"), EventTopic("0xc3d58168c5ae7397731d063d5bbf3d657854427343f4c083240f7aacaa2d0f62")]
public class Erc1155TransferEvent : IEventDTO
{
    [Parameter("address", "operator", 1, true)]
    public virtual string Operator { get; set; }

    [Parameter("address", "from", 2, true)]
    public virtual string From { get; set; }

    [Parameter("address", "to", 3, true)]
    public virtual string To { get; set; }

    [Parameter("uint256", "id", 4, false)]
    public virtual BigInteger TokenId { get; set; }

    [Parameter("uint256", "value", 5, false)]
    public virtual BigInteger Amount { get; set; }
}
