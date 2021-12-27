using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Newtonsoft.Json.Linq;
using SmartMetaData.Extensions;
using SmartMetaData.Models.Entities;

namespace SmartMetaData.Services;

public class TokenMetadataParser : ITokenMetadataParser
{
    private readonly IReadOnlyCollection<NftAttributeSearch> _attributeSearches = new NftAttributeSearch[]
    {
        new ("name", "name"),
        new ("description", "description"),
        new ("image", "image"),
        new ("animation_url", "animation_url"),
        new ("external_url", "external_url"),
        new ("background_color", "background_color"),
        new ("edition_id", "attributes.edition_id", "edition_number"),
        new ("total_edition", "attributes.total_edition", "edition_count"),
        new ("creator_name", "attributes.by"),
        new ("creator_external_page_url", "creator.external_url"),
    };

    public Result<NftTokenMetadata> Parse(string tokenMetadata)
    {
        var allAttributesResult = GetAllAttributes(tokenMetadata);
        if (allAttributesResult.IsFailure)
            return Result.Failure<NftTokenMetadata>(allAttributesResult.Error);

        var allAttributes = allAttributesResult.Value;

        string FindAttribute(string name)
        {
            var search = _attributeSearches.Single(x => x.Name == name);
            return allAttributes.FirstOrDefault(search.IsMatch)?.Value;
        }

        var parsedModel = new NftTokenMetadata();
        parsedModel.Name = FindAttribute("name");
        parsedModel.Description = FindAttribute("description");
        parsedModel.Image = FindAttribute("image");
        parsedModel.AnimationUrl = FindAttribute("animation_url");
        parsedModel.ExternalUrl = FindAttribute("external_url");
        parsedModel.BackgroundColor = FindAttribute("background_color");
        parsedModel.EditionId = long.TryParse(FindAttribute("edition_id"), out var editionId) ? editionId : null;
        parsedModel.TotalEdition = long.TryParse(FindAttribute("total_edition"), out var totalEdition) ? totalEdition : null;
        parsedModel.CreatorName = FindAttribute("creator_name");
        parsedModel.CreatorExternalPageUrl = FindAttribute("creator_external_page_url");

        var matchedAttributes = allAttributes.Where(attribute => _attributeSearches.Any(search => search.IsMatch(attribute))).ToArray();
        var nonMatchedAttributes = allAttributes.Except(matchedAttributes).ToArray();
        parsedModel.Attributes = nonMatchedAttributes;

        return parsedModel;
    }

    private static Result<IList<NftAttribute>> GetAllAttributes(string tokenMetadata)
    {
        try
        {
            if (tokenMetadata.ElementAt(0) == '"')
            {
                tokenMetadata = Regex.Unescape(tokenMetadata);
                tokenMetadata = tokenMetadata.Remove(tokenMetadata.Length - 1);
                tokenMetadata = tokenMetadata.Substring(1, tokenMetadata.Length - 1);
            }

            var jObj = JObject.Parse(tokenMetadata);

            var allJTokens = jObj.Flatten();
            var allAttributes = allJTokens.Select(x => new NftAttribute
            {
                Path = x.Path,
                Value = x.ToString(),
            }).ToList();
            return allAttributes;
        }
        catch (Exception e)
        {
            return Result.Failure<IList<NftAttribute>>($"Error during parsing attributes: {e.Message}");
        }
    }
}
