using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;
using AlephVault.Unity.EVMGames.Nethereum.RPC.Eth.DTOs;

namespace AlephVault.Unity.EVMGames.Nethereum.RPC.Eth.Transactions
{
    public interface IEthSendTransaction
    {
        RpcRequest BuildRequest(TransactionInput input, object id = null);
        Task<string> SendRequestAsync(TransactionInput input, object id = null);
    }
}