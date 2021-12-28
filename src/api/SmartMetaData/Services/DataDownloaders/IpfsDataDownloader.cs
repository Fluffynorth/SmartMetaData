using CSharpFunctionalExtensions;
using Ipfs.Http;

namespace SmartMetaData.Services.DataDownloaders;

public class IpfsDataDownloader : IDataDownloader
{
    private const string BaseIpfsUrl = "https://ipfs.io/";
    private const string Prefix = "ipfs://";

    public async Task<Result<string>> GetString(string uri)
    {
        if (string.IsNullOrEmpty(uri))
            return Result.Failure<string>("Uri is null or empty");

        if (!uri.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase))
            return Result.Failure<string>("Uri scheme is not supported by this data downloader");

        uri = uri.Substring(Prefix.Length);

        try
        {
            var ipfs = new IpfsClient(BaseIpfsUrl);
            string text = await ipfs.FileSystem.ReadAllTextAsync(uri);
            return text;
        }
        catch (Exception e)
        {
            return Result.Failure<string>($"Error during data downloading: {e.Message}");
        }
    }
}
