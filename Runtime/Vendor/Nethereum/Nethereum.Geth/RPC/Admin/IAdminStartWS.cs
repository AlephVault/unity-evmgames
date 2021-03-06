using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;

namespace AlephVault.Unity.EVMGames.Nethereum.Geth.RPC.Admin
{
    public interface IAdminStartWS
    {
        RpcRequest BuildRequest(string host, int port, string cors, string api, object id = null);
        Task<bool> SendRequestAsync(object id = null);
        Task<bool> SendRequestAsync(string host, object id = null);
        Task<bool> SendRequestAsync(string host, int port, object id = null);
        Task<bool> SendRequestAsync(string host, int port, string cors, object id = null);
        Task<bool> SendRequestAsync(string host, int port, string cors, string api, object id = null);
    }
}