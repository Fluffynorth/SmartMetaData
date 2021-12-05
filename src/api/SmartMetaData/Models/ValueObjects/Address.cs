using CSharpFunctionalExtensions;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;

namespace SmartMetaData.Models.ValueObjects;

public class Address : ValueObject<Address>
{
    private Address(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static implicit operator string(Address address) => address.Value;
    public static explicit operator Address(string address) => Create(address).Value;

    public static Result<Address> Create(string address)
    {
        const string longFormatPrefix = "0x000000000000000000000000";
        const int longFormatLength = 66;

        if (address.IsAnEmptyAddress())
            return Result.Failure<Address>("Address is empty");

        address = address.EnsureHexPrefix();
        if (address.Length == longFormatLength && address.StartsWith(longFormatPrefix))
            address = address.Substring(longFormatPrefix.Length).EnsureHexPrefix();

        if (!address.IsValidEthereumAddressHexFormat() || !address.IsValidEthereumAddressLength())
            return Result.Failure<Address>("Invalid address value");

        return new Address(address);
    }

    public override string ToString() => Value;

    protected override bool EqualsCore(Address other)
        => string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);

    protected override int GetHashCodeCore() => Value.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
}
