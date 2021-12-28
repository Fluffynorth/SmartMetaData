namespace SmartMetaData.Models.Entities;

public class NftAttribute
{
    public string Path { get; set; }
    public string Value { get; set; }

    public static implicit operator NftAttribute((string, string) kvp)
        => new NftAttribute() { Path = kvp.Item1, Value = kvp.Item2 };
}
