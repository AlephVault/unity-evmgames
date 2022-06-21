using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;

namespace AlephVault.Unity.EVMGames.Nethereum.Besu.RPC.Debug
{
    /// <Summary>
    ///     Remix uses debug_storageRangeAt to implement debugging. Use the Debugger tab in Remix rather than calling
    ///     debug_storageRangeAt directly.
    ///     Returns the contract storage for the specified range.
    /// </Summary>
    public class DebugStorageRangeAt : RpcRequestResponseHandler<JObject>, IDebugStorageRangeAt
    {
        public DebugStorageRangeAt(IClient client) : base(client, ApiMethods.debug_storageRangeAt.ToString())
        {
        }

        public Task<JObject> SendRequestAsync(string blockHash, int txIndex, string contractAddress,
            string startKeyHash, int limitStorageEntries, object id = null)
        {
            return base.SendRequestAsync(id, blockHash, txIndex, contractAddress, startKeyHash,
                limitStorageEntries);
        }

        public RpcRequest BuildRequest(string blockHash, int txIndex, string contractAddress, string startKeyHash,
            int limitStorageEntries, object id = null)
        {
            return base.BuildRequest(id, blockHash, txIndex, contractAddress, startKeyHash, limitStorageEntries);
        }
    }
}