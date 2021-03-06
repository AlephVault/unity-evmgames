using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;
using AlephVault.Unity.EVMGames.Nethereum.RPC.Infrastructure;

namespace AlephVault.Unity.EVMGames.Nethereum.Parity.RPC.Admin
{
    /// <Summary>
    ///     parity_consensusCapability
    ///     Returns information on current consensus capability.
    ///     Parameters
    ///     None
    ///     Returns
    ///     Object - or String - Either "capable", {"capableUntil":N}, {"incapableSince":N} or "unknown" (N is a block number).
    ///     Example
    ///     Request
    ///     curl --data '{"method":"parity_consensusCapability","params":[],"id":1,"jsonrpc":"2.0"}' -H "Content-Type:
    ///     application/json" -X POST localhost:8545
    ///     Response
    ///     {
    ///     "id": 1,
    ///     "jsonrpc": "2.0",
    ///     "result": "capable"
    ///     }
    /// </Summary>
    public class ParityConsensusCapability : GenericRpcRequestResponseHandlerNoParam<string>, IParityConsensusCapability
    {
        public ParityConsensusCapability(IClient client) : base(client,
            ApiMethods.parity_consensusCapability.ToString())
        {
        }
    }
}