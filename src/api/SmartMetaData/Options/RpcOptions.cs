using SmartMetaData.Models.Enums;

namespace SmartMetaData.Options;

public class RpcOptions
{
    public string? InfuraProjectId { get; set; }
    public NetworkOptions[]? Networks { get; set; }

    public Uri? GetRpcUrl(EthereumNetwork value)
    {
        var infuraBaseUrl = Networks?.FirstOrDefault(x => x.Id == value)?.InfuraBaseUrl;
        if (infuraBaseUrl == null)
            return null;

        var rpcUrl = new Uri(infuraBaseUrl, InfuraProjectId);
        return rpcUrl;
    }
}
