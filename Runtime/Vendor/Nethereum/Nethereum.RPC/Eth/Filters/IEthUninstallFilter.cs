using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Nethereum.Hex.HexTypes;
using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;

namespace AlephVault.Unity.EVMGames.Nethereum.RPC.Eth.Filters
{
    public interface IEthUninstallFilter
    {
        RpcRequest BuildRequest(HexBigInteger filterId, object id = null);
        Task<bool> SendRequestAsync(HexBigInteger filterId, object id = null);
    }
}