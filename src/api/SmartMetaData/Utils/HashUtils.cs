using System.Numerics;
using System.Text;
using Murmur;
using SmartMetaData.Models.ValueObjects;

namespace SmartMetaData.Utils;

public static class HashUtils
{
    public static Guid HashToken(Address contractAddress, BigInteger tokenId)
    {
        if (string.IsNullOrEmpty(contractAddress))
            throw new ArgumentNullException(contractAddress);

        var compoundString = $"{contractAddress.ToString().ToLowerInvariant()}+{tokenId.ToString().ToLowerInvariant()}";

        return HashString(compoundString);
    }

    public static Guid HashString(string stringToHash)
    {
        if (string.IsNullOrEmpty(stringToHash))
            throw new ArgumentNullException(stringToHash);

        var bytesToHash = Encoding.UTF8.GetBytes(stringToHash);
        return HashBytes(bytesToHash);
    }

    public static Guid HashBytes(byte[] bytesToHash)
    {
        var murmur128 = MurmurHash.Create128();
        byte[] hashBytes = murmur128.ComputeHash(bytesToHash);
        var hashString = string.Concat(Array.ConvertAll(hashBytes, x => x.ToString("x2")));

        return Guid.Parse(hashString);
    }
}
