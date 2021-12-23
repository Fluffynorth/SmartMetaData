using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.Events;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Services.EventProcessor;

public class Erc1155EventProcessor : GenericEventProcessor<Erc1155TransferEvent>
{
    protected override EventLog<Erc1155TransferEvent> ParseEventLog(FilterLog rawEvent)
    {
        if (rawEvent?.Topics == null)
            return null;

        try
        {
            var dataArray = Decoder.ExtractArray(rawEvent.Data);
            var @event = rawEvent.Topics.Length switch
            {
                1 => new Erc1155TransferEvent
                {
                    Operator = Address.Create(dataArray[0]).Value,
                    From = Address.Create(dataArray[1]).Value,
                    To = Address.Create(dataArray[2]).Value,
                    TokenId = Decoder.DecodeBigInteger(dataArray[3]).Value,
                    Amount = Decoder.DecodeBigInteger(dataArray[4]).Value
                },
                2 => new Erc1155TransferEvent
                {
                    Operator = Address.Create(rawEvent.Topics[1] as string).Value,
                    From = Address.Create(dataArray[0]).Value,
                    To = Address.Create(dataArray[1]).Value,
                    TokenId = Decoder.DecodeBigInteger(dataArray[2]).Value,
                    Amount = Decoder.DecodeBigInteger(dataArray[3]).Value
                },
                3 => new Erc1155TransferEvent
                {
                    Operator = Address.Create(rawEvent.Topics[1] as string).Value,
                    From = Address.Create(rawEvent.Topics[2] as string).Value,
                    To = Address.Create(dataArray[0]).Value,
                    TokenId = Decoder.DecodeBigInteger(dataArray[1]).Value,
                    Amount = Decoder.DecodeBigInteger(dataArray[2]).Value
                },
                4 => new Erc1155TransferEvent
                {
                    Operator = Address.Create(rawEvent.Topics[1] as string).Value,
                    From = Address.Create(rawEvent.Topics[2] as string).Value,
                    To = Address.Create(rawEvent.Topics[3] as string).Value,
                    TokenId = Decoder.DecodeBigInteger(dataArray[0]).Value,
                    Amount = Decoder.DecodeBigInteger(dataArray[1]).Value
                },
                _ => throw new ArgumentOutOfRangeException(nameof(rawEvent.Topics)),
            };
            return CreateEventLog(rawEvent, @event);
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected override IReadOnlyCollection<TokenTransferDetails> ConvertToTokenTransferDetails(EventLog<Erc1155TransferEvent> eventLog)
    {
        var tokenTransferDetails = TokenTransferDetails.Create(eventLog.Event.From, eventLog.Event.To, eventLog.Log.Address, eventLog.Event.TokenId, eventLog.Event.Amount, TokenType.Erc1155);
        return tokenTransferDetails.IsSuccess ? new [] { tokenTransferDetails.Value } : Array.Empty<TokenTransferDetails>();
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
