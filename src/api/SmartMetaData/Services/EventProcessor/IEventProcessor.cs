using Nethereum.Contracts;
using Nethereum.JsonRpc.Client;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Services.EventProcessor;

public interface IEventProcessor
{
    Task<IReadOnlyCollection<EventLog<TokenTransferDetails>>> GetTokenTransfers(IClient rpcClient, Address fromAddress, Address toAddress);
}
