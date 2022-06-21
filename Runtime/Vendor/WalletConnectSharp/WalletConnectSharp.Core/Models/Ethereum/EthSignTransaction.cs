using Newtonsoft.Json;

namespace AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Models.Ethereum
{
    public sealed class EthSignTransaction : JsonRpcRequest
    {
        [JsonProperty("params")] 
        private TransactionData[] _parameters;

        [JsonIgnore]
        public TransactionData[] Parameters => _parameters;

        public EthSignTransaction(params TransactionData[] transactionDatas) : base()
        {
            this.Method = "eth_signTransaction";
            this._parameters = transactionDatas;
        }
    }
}