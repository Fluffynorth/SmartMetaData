using CSharpFunctionalExtensions;

namespace SmartMetaData.Services.DataDownloaders;

public class DataDownloaderFactory : IDataDownloaderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DataDownloaderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Result<IDataDownloader> Create(string protocol)
    {
        switch (protocol)
        {
            case "http":
            case "https":
                return _serviceProvider.GetRequiredService<HttpDataDownloader>();
            case "ipfs":
                return _serviceProvider.GetRequiredService<IpfsDataDownloader>();
            default:
                return Result.Failure<IDataDownloader>("Unknown protocol scheme");
        }
    }
}
