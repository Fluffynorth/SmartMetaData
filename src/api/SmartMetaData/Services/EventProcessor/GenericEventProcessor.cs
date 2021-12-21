using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Attributes;
using SmartMetaData.Extensions;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Services.Rlp;

namespace SmartMetaData.Services.EventProcessor;

public abstract class GenericEventProcessor<T> : IEventProcessor
        where T : class, IEventDTO
{
    protected readonly IRlpDecoder Decoder = new RlpDecoderService();

    public async Task<IReadOnlyCollection<EventLog<TokenTransferDetails>>> GetTokenTransfers(IClient rpcClient, Address fromAddress, Address toAddress)
    {
        var eventTopic = typeof(T).GetAttribute<EventTopicAttribute>().Topic;
        var rawLogs = await rpcClient.SendRequestAsync<FilterLog[]>(new RpcRequest(1, "eth_getLogs", new
        {
            fromBlock = BigInteger.Zero.ToHexBigInteger().HexValue,
            toBlock = "latest",
            topics = new string[]
            {
                eventTopic,
                fromAddress?.ToLongFormatString(),
                toAddress?.ToLongFormatString(),
            },
        }));
        var parsedLogs = rawLogs.Where(x => !x.Removed).Select(ParseEventLog).Where(x => x != null).ToArray();
        var transferDetails = parsedLogs.SelectMany(@event => ConvertToTokenTransferDetails(@event).Select(x => new EventLog<TokenTransferDetails>(x, @event.Log)))
            .Where(x => x != null).ToArray();

        return transferDetails;
    }

    protected EventLog<TEvent> CreateEventLog<TEvent>(FilterLog logEntry, TEvent @event)
    {
        var log = new FilterLog
        {
            Address = logEntry.Address,
            Data = logEntry.Data,
            Removed = logEntry.Removed,
            Topics = logEntry.Topics.Select(t => (object) t).ToArray(),
            BlockHash = logEntry.BlockHash,
            BlockNumber = new HexBigInteger(logEntry.BlockNumber),
            LogIndex = new HexBigInteger(logEntry.LogIndex),
            TransactionHash = logEntry.TransactionHash,
            TransactionIndex = new HexBigInteger(logEntry.TransactionIndex),
        };
        return new EventLog<TEvent>(@event, log);
    }

    protected abstract EventLog<T> ParseEventLog(FilterLog rawEvent);
    protected abstract IReadOnlyCollection<TokenTransferDetails> ConvertToTokenTransferDetails(EventLog<T> @event);
}
