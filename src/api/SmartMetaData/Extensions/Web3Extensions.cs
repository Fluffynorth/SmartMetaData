using CSharpFunctionalExtensions;
using Nethereum.Contracts;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Extensions;

public static class Web3Extensions
{
    public static async Task<Result<string>> SafeQuery<TFunction>(this Web3 web3, Address contractAddress, TFunction message)
        where TFunction : FunctionMessage, new()
    {
        try
        {
            var handler = web3.Eth.GetContractQueryHandler<TFunction>();
            var response = await handler.QueryAsync<string>(contractAddress, message);
            return Result.Success(response);
        }
        catch (RpcResponseException e)
        {
            return Result.Failure<string>(e.RpcError.Message);
        }
    }
}
