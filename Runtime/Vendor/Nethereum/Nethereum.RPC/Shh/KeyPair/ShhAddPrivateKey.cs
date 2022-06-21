﻿using AlephVault.Unity.EVMGames.Nethereum.Hex.HexConvertors.Extensions;
using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;
using AlephVault.Unity.EVMGames.Nethereum.RPC.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlephVault.Unity.EVMGames.Nethereum.RPC.Shh.KeyPair
{
    public class ShhAddPrivateKey : RpcRequestResponseHandler<string>, IShhAddPrivateKey
    {
        public ShhAddPrivateKey(IClient client) : base(client, ApiMethods.shh_addPrivateKey.ToString())
        {
        }

        public RpcRequest BuildRequest(string privateKey, object id = null)
        {
            if (string.IsNullOrEmpty(privateKey)) throw new ArgumentNullException(nameof(privateKey));
            return base.BuildRequest(id, privateKey.EnsureHexPrefix());
        }

        public Task<string> SendRequestAsync(string privateKey, object id = null)
        {
            if (string.IsNullOrEmpty(privateKey)) throw new ArgumentNullException(nameof(privateKey));
            return base.SendRequestAsync(id, privateKey.EnsureHexPrefix());
        }
    }
}