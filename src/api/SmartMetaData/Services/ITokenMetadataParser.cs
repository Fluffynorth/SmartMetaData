using CSharpFunctionalExtensions;
using SmartMetaData.Models.Entities;

namespace SmartMetaData.Services;

public interface ITokenMetadataParser
{
    Result<NftTokenMetadata> Parse(string tokenMetadata);
}
