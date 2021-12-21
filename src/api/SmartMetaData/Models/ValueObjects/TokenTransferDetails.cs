using System.Numerics;
using CSharpFunctionalExtensions;
using SmartMetaData.Models.Enums;

namespace SmartMetaData.Models.ValueObjects;

public class TokenTransferDetails : ValueObject<TokenTransferDetails>
{
    public Address FromAddress { get; private set; }
    public Address ToAddress { get; private set; }
    public Address ContractAddress { get; private set; }
    public BigInteger TokenId { get; private set; }
    public BigInteger TokensAmount { get; private set; }
    public TokenType TokenType { get; private set; }

    private TokenTransferDetails(Address fromAddress, Address toAddress, Address contractAddress, BigInteger tokenId, BigInteger tokensAmount, TokenType tokenType)
    {
        FromAddress = fromAddress;
        ToAddress = toAddress;
        ContractAddress = contractAddress;
        TokenId = tokenId;
        TokensAmount = tokensAmount;
        TokenType = tokenType;
    }

    public static Result<TokenTransferDetails> Create(string fromAddress, string toAddress, string contractAddress, BigInteger tokenId, BigInteger tokenAmount, TokenType tokenType)
    {
        var fromAddressResult = Address.Create(fromAddress);
        var toAddressResult = Address.Create(toAddress);
        var contractAddressResult = Address.Create(contractAddress);
        if (fromAddressResult.IsFailure || toAddressResult.IsFailure || contractAddressResult.IsFailure)
            return Result.Failure<TokenTransferDetails>("Invalid address");

        if (tokenId < 0)
            return Result.Failure<TokenTransferDetails>("Invalid token id");

        if (tokenAmount <= 0)
            return Result.Failure<TokenTransferDetails>("Invalid token amount");

        if (tokenType == TokenType.Erc721 && tokenAmount > 1)
            return Result.Failure<TokenTransferDetails>("Invalid amount for ERC-721 token");

        return new TokenTransferDetails(fromAddressResult.Value, toAddressResult.Value, contractAddressResult.Value, tokenId, tokenAmount, tokenType);
    }

    protected override bool EqualsCore(TokenTransferDetails other)
        => Equals(FromAddress, other.FromAddress) && Equals(ToAddress, other.ToAddress) && Equals(ContractAddress, other.ContractAddress) && TokenId.Equals(other.TokenId) && TokensAmount.Equals(other.TokensAmount) && TokenType == other.TokenType;

    protected override int GetHashCodeCore()
        => HashCode.Combine(FromAddress, ToAddress, ContractAddress, TokenId, TokensAmount, (int) TokenType);
}
