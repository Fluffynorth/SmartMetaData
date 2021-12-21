using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using SmartMetaData.Attributes;

namespace SmartMetaData.Models.Events;

[Event("TransferBatch"), EventTopic("0x4a39dc06d4c0dbc64b70af90fd698a233a518aa5d07e595d983b8c0526c8f7fb")]
public class Erc1155BatchTransferEvent : IEventDTO
{
    [Parameter("address", "operator", 1, true)]
    public virtual string Operator { get; set; }

    [Parameter("address", "from", 2, true)]
    public virtual string From { get; set; }

    [Parameter("address", "to", 3, true)]
    public virtual string To { get; set; }

    [Parameter("uint256[]", "ids", 4, false)]
    public virtual BigInteger[] TokenIds { get; set; }

    [Parameter("uint256[]", "values", 5, false)]
    public virtual BigInteger[] Amounts { get; set; }
}
