using SmartMetaData.Models.Enums;

namespace SmartMetaData.Options;

public class ChainOptions
{
    public EthereumChain Id { get; set; }
    public Uri? InfuraBaseUrl { get; set; }
}
