using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Models.Enums;
using SmartMetaData.Models.Events;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Services.EventProcessor;

public class Erc721EventProcessor : GenericEventProcessor<Erc721TransferEvent>
{
    protected override EventLog<Erc721TransferEvent> ParseEventLog(FilterLog rawEvent)
    {
        if (rawEvent?.Topics == null)
            return null;

        try
        {
            var dataArray = Decoder.ExtractArray(rawEvent.Data);
            var @event = rawEvent.Topics.Length switch
            {
                1 => new Erc721TransferEvent
                {
                    From = Address.Create(dataArray[0]).Value,
                    To = Address.Create(dataArray[1]).Value,
                    TokenId = Decoder.DecodeBigInteger(dataArray[2]).Value,
                },
                2 => new Erc721TransferEvent
                {
                    From = Address.Create(rawEvent.Topics[1] as string).Value,
                    To = Address.Create(dataArray[0]).Value,
                    TokenId = Decoder.DecodeBigInteger(dataArray[1]).Value,
                },
                3 => new Erc721TransferEvent
                {
                    From = Address.Create(rawEvent.Topics[1] as string).Value,
                    To = Address.Create(rawEvent.Topics[2] as string).Value,
                    TokenId = Decoder.DecodeBigInteger(dataArray[0]).Value,
                },
                4 => new Erc721TransferEvent
                {
                    From = Address.Create(rawEvent.Topics[1] as string).Value,
                    To = Address.Create(rawEvent.Topics[2] as string).Value,
                    TokenId = Decoder.DecodeBigInteger(rawEvent.Topics[3] as string).Value,
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

    protected override IReadOnlyCollection<TokenTransferDetails> ConvertToTokenTransferDetails(EventLog<Erc721TransferEvent> eventLog)
    {
        var tokenTransferDetails = TokenTransferDetails.Create(eventLog.Event.From, eventLog.Event.To, eventLog.Log.Address, eventLog.Event.TokenId, 1, TokenType.Erc721);
        return tokenTransferDetails.IsSuccess ? new [] { tokenTransferDetails.Value } : Array.Empty<TokenTransferDetails>();
    }
}
