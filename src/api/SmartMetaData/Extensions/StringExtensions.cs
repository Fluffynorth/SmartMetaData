namespace SmartMetaData.Extensions;

public static class StringExtensions
{
    public static IEnumerable<string> SplitByCharactersCount(this string str, int iterateCount)
    {
        var words = new List<string>();

        for (var i = 0; i < str.Length; i += iterateCount)
        {
            words.Add(str.Length - i >= iterateCount
                ? str.Substring(i, iterateCount)
                : str.Substring(i, str.Length - i));
        }

        return words;
    }
}
