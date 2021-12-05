using SmartMetaData.Domain.Models.Enums;

namespace SmartMetaData.Infrastructure.Options;

public class NetworkOptions
{
    public EthereumNetwork Id { get; set; }
    public Uri? InfuraBaseUrl { get; set; }
}
