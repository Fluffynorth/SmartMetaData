using SmartMetaData.Domain.Models.Enums;

namespace SmartMetaData.Infrastructure.Options;

public class RpcOptions
{
    public NetworkOptions[]? Networks { get; set; }

    public Uri? GetRpcUrl(EthereumNetwork value)
    {
        var networkOptions = Networks?.FirstOrDefault(x => x.Id == value);
        var rpcUrl = networkOptions?.RpcNodeUrls?.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
        return rpcUrl;
    }
}
