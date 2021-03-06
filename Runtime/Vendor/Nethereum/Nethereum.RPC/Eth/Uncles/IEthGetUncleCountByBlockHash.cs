using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Nethereum.Hex.HexTypes;
using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;

namespace AlephVault.Unity.EVMGames.Nethereum.RPC.Eth.Uncles
{
    public interface IEthGetUncleCountByBlockHash
    {
        RpcRequest BuildRequest(string hash, object id = null);
        Task<HexBigInteger> SendRequestAsync(string hash, object id = null);
    }
}