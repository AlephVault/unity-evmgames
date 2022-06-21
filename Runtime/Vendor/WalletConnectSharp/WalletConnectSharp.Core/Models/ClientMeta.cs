using System;
using Newtonsoft.Json;

namespace AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Models
{
    [Serializable]
    public class ClientMeta
    {
        [JsonProperty("description")]
        public string Description;

        [JsonProperty("url")]
        public string URL;

        [JsonProperty("icons")]
        public string[] Icons;

        [JsonProperty("name")]
        public string Name;
    }
}