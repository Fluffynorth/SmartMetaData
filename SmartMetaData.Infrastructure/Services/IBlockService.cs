using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Domain.Models.Enums;

namespace SmartMetaData.Infrastructure.Services;

public interface IBlockService
{
    Task<Block> GetLatestBlock(EthereumNetwork network);
    Task<Block> GetBlockByNumber(BigInteger blockNumber, EthereumNetwork network);
}
