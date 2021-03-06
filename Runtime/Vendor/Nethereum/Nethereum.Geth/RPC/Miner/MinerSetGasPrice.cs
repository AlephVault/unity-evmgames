using System;
using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.Nethereum.Hex.HexTypes;
using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;

namespace AlephVault.Unity.EVMGames.Nethereum.Geth.RPC.Miner
{
    /// <Summary>
    ///     Sets the minimal accepted gas price when mining transactions. Any transactions that are below this limit are
    ///     excluded from the mining process.
    /// </Summary>
    public class MinerSetGasPrice : RpcRequestResponseHandler<bool>, IMinerSetGasPrice
    {
        public MinerSetGasPrice(IClient client) : base(client, ApiMethods.miner_setGasPrice.ToString())
        {
        }

        public RpcRequest BuildRequest(HexBigInteger price, object id = null)
        {
            if (price == null) throw new ArgumentNullException(nameof(price));
            return base.BuildRequest(id, price);
        }

        public Task<bool> SendRequestAsync(HexBigInteger price, object id = null)
        {
            if (price == null) throw new ArgumentNullException(nameof(price));
            return base.SendRequestAsync(id, price);
        }
    }
}