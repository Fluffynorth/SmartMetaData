using CSharpFunctionalExtensions;

namespace SmartMetaData.Services.DataDownloaders;

public class HttpDataDownloader : IDataDownloader
{
    private const string HttpPrefix = "http://";
    private const string HttpsPrefix = "https://";

    public async Task<Result<string>> GetString(string uri)
    {
        if (string.IsNullOrEmpty(uri))
            return Result.Failure<string>("Uri is null or empty");

        if (!uri.StartsWith(HttpPrefix, StringComparison.InvariantCultureIgnoreCase) && !uri.StartsWith(HttpsPrefix, StringComparison.InvariantCultureIgnoreCase))
            return Result.Failure<string>("Uri scheme is not supported by this data downloader");

        try
        {
            using var webClient = new HttpClient();
            var text = await webClient.GetStringAsync(uri);
            return text;
        }
        catch (Exception e)
        {
            return Result.Failure<string>($"Error during data downloading: {e.Message}");
        }
    }
}
