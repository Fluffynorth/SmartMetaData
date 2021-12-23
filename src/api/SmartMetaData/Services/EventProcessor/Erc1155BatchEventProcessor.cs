using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.Events;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Services.EventProcessor;

public class Erc1155BatchEventProcessor : GenericEventProcessor<Erc1155BatchTransferEvent>
{
    protected override EventLog<Erc1155BatchTransferEvent> ParseEventLog(FilterLog rawEvent)
    {
        if (rawEvent?.Topics == null || rawEvent.Topics.Length != 4)
            return null;

        try
        {
            var dataArray = Decoder.ExtractArray(rawEvent.Data).Skip(3).ToArray();
            var tokensAmount = dataArray.Length / 2;
            var ids = dataArray.Take(tokensAmount).ToArray();
            var amounts = dataArray.TakeLast(tokensAmount).ToArray();

            var @event = new Erc1155BatchTransferEvent
            {
                Operator = Address.Create(rawEvent.Topics[1] as string).Value,
                From = Address.Create(rawEvent.Topics[2] as string).Value,
                To = Address.Create(rawEvent.Topics[3] as string).Value,
                TokenIds = ids.Select(id => Decoder.DecodeBigInteger(id).Value).ToArray(),
                Amounts = amounts.Select(amount => Decoder.DecodeBigInteger(amount).Value).ToArray(),
            };
            return CreateEventLog(rawEvent, @event);
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected override IReadOnlyCollection<TokenTransferDetails> ConvertToTokenTransferDetails(EventLog<Erc1155BatchTransferEvent> eventLog)
    {
        if (eventLog.Event.TokenIds == null ||
            eventLog.Event.Amounts == null ||
            eventLog.Event.TokenIds.Length != eventLog.Event.Amounts.Length)
        {
            return Array.Empty<TokenTransferDetails>();
        }

        var transferDetails = new List<TokenTransferDetails>();

        for (int i = 0; i < eventLog.Event.TokenIds.Length; i++)
        {
            var tokenId = eventLog.Event.TokenIds[i];
            var amount = eventLog.Event.Amounts[i];
            var elementResult = TokenTransferDetails.Create(eventLog.Event.From, eventLog.Event.To, eventLog.Log.Address, tokenId, amount, TokenType.Erc1155);
            if (elementResult.IsFailure)
            {
                return Array.Empty<TokenTransferDetails>();
            }

            transferDetails.Add(elementResult.Value);
        }
        return transferDetails;
    }

    protected override string[] GetTopics(Address fromAddress, Address toAddress)
        => new string[]
        {
            GetEventTopic(),
            null, // operator
            fromAddress?.ToLongFormatString(),
            toAddress?.ToLongFormatString(),
        };
}
