using Newtonsoft.Json;

namespace AlephVault.Unity.EVMGames.Nethereum.KeyStore.Model
{
    public class ScryptParams : KdfParams
    {
        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("r")]
        public int R { get; set; }

        [JsonProperty("p")]
        public int P { get; set; }
    }
}