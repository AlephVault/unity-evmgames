using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;
using AlephVault.Unity.EVMGames.Nethereum.RPC.Infrastructure;

namespace AlephVault.Unity.EVMGames.Nethereum.Geth.RPC.Admin
{
    /// <Summary>
    ///     The stopRPC administrative method closes the currently open HTTP RPC endpoint. As the node can only have a single
    ///     HTTP endpoint running, this method takes no parameters, returning a boolean whether the endpoint was closed or not.
    /// </Summary>
    public class AdminStopRPC : GenericRpcRequestResponseHandlerNoParam<bool>, IAdminStopRPC
    {
        public AdminStopRPC(IClient client) : base(client, ApiMethods.admin_stopRPC.ToString())
        {
        }
    }
}