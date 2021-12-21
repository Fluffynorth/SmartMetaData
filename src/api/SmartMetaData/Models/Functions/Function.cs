using Nethereum.ABI.FunctionEncoding;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using SmartMetaData.Attributes;
using SmartMetaData.Extensions;

namespace SmartMetaData.Models.Functions;

public abstract class Function : FunctionMessage
{
    public string Encode()
    {
        var hash = GetHash().EnsureHexPrefix();
        var encoder = new FunctionCallEncoder();
        var encodedParameters = encoder.EncodeParametersFromTypeAttributes(GetType(), this);
        var encodedCall = encoder.EncodeRequest(hash, encodedParameters.ToHex());
        return encodedCall;
    }

    public Function Decode(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input));

        input = input.EnsureHexPrefix();

        var hash = GetHash().EnsureHexPrefix();
        if (!input.StartsWith(hash, StringComparison.InvariantCultureIgnoreCase))
            throw new ArgumentException("Input does not belong to this function", nameof(input));

        var functionType = GetType();
        var instance = (Function)Activator.CreateInstance(functionType);
        if (input.Equals(hash, StringComparison.InvariantCultureIgnoreCase))
            return instance;

        input = input.Substring(hash.Length);

        var parameterAttribute = PropertiesExtractor.GetPropertiesWithParameterAttribute(functionType);
        var decoder = new FunctionCallDecoder();
        decoder.DecodeAttributes(input, instance, parameterAttribute.ToArray());
        return instance;
    }


    public string GetHash() => GetType().GetAttribute<FunctionHashAttribute>().Hash;
}
