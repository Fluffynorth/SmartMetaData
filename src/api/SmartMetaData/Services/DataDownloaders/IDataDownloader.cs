using CSharpFunctionalExtensions;

namespace SmartMetaData.Services.DataDownloaders;

public interface IDataDownloader
{
    Task<Result<string>> GetString(string uri);
}
