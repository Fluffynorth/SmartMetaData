using Newtonsoft.Json.Linq;

namespace SmartMetaData.Extensions;

public static class JsonExtensions
{
    public static IEnumerable<JToken> Flatten(this JToken jToken)
    {
        var children = jToken.Children().ToArray();
        return children.Length == 0 ? new[] { jToken } : children.SelectMany(Flatten);
    }
}
