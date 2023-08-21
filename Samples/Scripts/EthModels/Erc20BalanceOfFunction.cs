using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;


namespace AlephVault.Unity.EVMGames
{
    namespace Samples
    {
        namespace EthModels
        {
            [Function("balanceOf", "uint256")]
            public class Erc20BalanceOfFunction : FunctionMessage
            {
                [Parameter("address", "_owner", 1)]
                public string Owner { get; set; }
            }
        }
    }
}