using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Models.Enums;

namespace SmartMetaData.Services;

public interface IBlockService
{
    Task<Block> GetLatestBlock(EthereumNetwork network);
    Task<Block> GetBlockByNumber(BigInteger blockNumber, EthereumNetwork network);
}
