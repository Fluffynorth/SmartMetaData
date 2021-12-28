namespace SmartMetaData.Models.Entities;

public class NftAttributeSearch
{
    public NftAttributeSearch(string name, params string[] paths)
    {
        Name = name;
        Paths = paths ?? Array.Empty<string>();
    }

    public string Name { get; }
    public string[] Paths { get; }

    public bool IsMatch(NftAttribute attribute)
        => Paths.Any(path => path == attribute.Path);
}
