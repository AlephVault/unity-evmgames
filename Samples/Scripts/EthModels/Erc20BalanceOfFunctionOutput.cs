using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;


namespace AlephVault.Unity.EVMGames
{
    namespace Samples
    {
        namespace EthModels
        {
            [FunctionOutput]
            public class BalanceOfOutputDTO : IFunctionOutputDTO
            {
                [Parameter("uint256", "balance", 1)]
                public BigInteger Balance { get; set; }
            }
        }
    }
}