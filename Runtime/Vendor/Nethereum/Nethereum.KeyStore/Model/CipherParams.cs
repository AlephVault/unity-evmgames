﻿using AlephVault.Unity.EVMGames.Nethereum.Hex.HexConvertors.Extensions;
using Newtonsoft.Json;

namespace AlephVault.Unity.EVMGames.Nethereum.KeyStore.Model
{
    public class CipherParams
    {
        public CipherParams()
        {
        }

        public CipherParams(byte[] iv)
        {
            Iv = iv.ToHex();
        }

        [JsonProperty("iv")]
        public string Iv { get; set; }
    }
}