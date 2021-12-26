using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using SmartMetaData.Models.Enums;

namespace SmartMetaData.Services;

public interface IBlockService
{
    Task<Block> GetLatestBlock(EthereumChain chain);
    Task<Block> GetBlockByNumber(EthereumChain chain, BigInteger blockNumber);
}
