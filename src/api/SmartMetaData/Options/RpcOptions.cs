using SmartMetaData.Models.Enums;

namespace SmartMetaData.Options;

public class RpcOptions
{
    public string InfuraProjectId { get; set; }
    public ChainOptions[] Chains { get; set; }

    public Uri GetRpcUrl(EthereumChain value)
    {
        var infuraBaseUrl = Chains?.FirstOrDefault(x => x.Id == value)?.InfuraBaseUrl;
        if (infuraBaseUrl == null)
            return null;

        var rpcUrl = new Uri(infuraBaseUrl, InfuraProjectId);
        return rpcUrl;
    }
}
