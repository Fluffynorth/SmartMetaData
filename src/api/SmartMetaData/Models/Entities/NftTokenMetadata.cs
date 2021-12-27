namespace SmartMetaData.Models.Entities;

public class NftTokenMetadata
{
    // ERC-721-Metadata
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    // Unofficial standard
    public string AnimationUrl { get; set; }
    public string ExternalUrl { get; set; }
    public string BackgroundColor { get; set; }

    // Other first-class attributes
    public long? EditionId { get; set; }
    public long? TotalEdition { get; set; }
    public string CreatorName { get; set; }
    public string CreatorImageUrl { get; set; }
    public string CreatorExternalPageUrl { get; set; }

    // Other second-class attributes
    public NftAttribute[] Attributes { get; set; }
}
