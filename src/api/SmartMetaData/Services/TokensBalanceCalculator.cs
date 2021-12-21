using System.Linq;
using System.Numerics;
using SmartMetaData.Models.Entities;
using SmartMetaData.Models.ValueObjects;
using SmartMetaData.Utils;

namespace SmartMetaData.Services;

public class TokensBalanceCalculator
{
    private readonly Address _address;
    private readonly IDictionary<Guid, TokenBalance> _tokenBalances = new Dictionary<Guid, TokenBalance>();

    public TokensBalanceCalculator(Address address)
    {
        _address = address;
    }

    public void Apply(TokenTransferDetails transfer)
    {
        if (transfer.FromAddress == transfer.ToAddress)
            return;

        if (transfer.ToAddress == _address)
        {
            Deposit(transfer);
        }
        else if (transfer.FromAddress == _address)
        {
            Withdraw(transfer);
        }
        else
        {
            throw new InvalidOperationException("Provided token transfer is not related to specified account");
        }
    }

    public IReadOnlyCollection<TokenBalance> GetBalance() => _tokenBalances.Values.Where(x => x.Amount > 0).ToArray();

    private void Deposit(TokenTransferDetails transfer)
    {
        var tokenId = HashUtils.HashToken(transfer.ContractAddress, transfer.TokenId);

        if (!_tokenBalances.ContainsKey(tokenId))
        {
            _tokenBalances.Add(tokenId, ToTokenBalance(transfer));
        }

        _tokenBalances[tokenId].Amount += transfer.TokensAmount;
    }

    private void Withdraw(TokenTransferDetails transfer)
    {
        var tokenId = HashUtils.HashToken(transfer.ContractAddress, transfer.TokenId);

        if (!_tokenBalances.ContainsKey(tokenId))
        {
            _tokenBalances.Add(tokenId, ToTokenBalance(transfer));
        }

        _tokenBalances[tokenId].Amount -= transfer.TokensAmount;
    }

    private TokenBalance ToTokenBalance(TokenTransferDetails transfer) => new TokenBalance
    {
        ContractAddress = transfer.ContractAddress,
        TokenId = transfer.TokenId,
    };
}
