using SmartMetaData.Models.Enums;

namespace SmartMetaData.Options;

public class NetworkOptions
{
    public EthereumNetwork Id { get; set; }
    public Uri? InfuraBaseUrl { get; set; }
}
