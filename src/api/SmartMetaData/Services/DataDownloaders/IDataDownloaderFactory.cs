using CSharpFunctionalExtensions;

namespace SmartMetaData.Services.DataDownloaders;

public interface IDataDownloaderFactory
{
    Result<IDataDownloader> Create(string protocol);
}
