using System.ComponentModel;

namespace SmartMetaData.Models.Enums;

public enum EthereumChain
{
    // usual ethereum chains
    [Description("Ethereum Mainnet")]
    Mainnet = 1,
    [Description("Ethereum Testnet - Ropsten")]
    Ropsten = 3,
    [Description("Ethereum Testnet - Rinkeby")]
    Rinkeby = 4,
    [Description("Ethereum Testnet - Kovan")]
    Kovan = 42,
    [Description("Ethereum Testnet - Goerli")]
    Goerli = 6284,

    // polygon
    [Description("Polygon Mainnet")]
    PolygonMainnet = 137,
    [Description("Polygon Testnet - Mumbai")]
    PolygonMumbai = 80001,
}
