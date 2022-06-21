using System;
using System.IO;
using System.Threading.Tasks;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Events;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Models;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Network;

namespace AlephVault.Unity.EVMGames.WalletConnectSharp.Unity
{
    public class WalletConnectUnitySession : WalletConnectSession
    {
        private WalletConnect unityObjectSource;

        private bool listenerAdded;

        public WalletConnectUnitySession(SavedSession savedSession, WalletConnect source, ITransport transport = null, ICipher cipher = null, EventDelegator eventDelegator = null) : base(savedSession, transport, cipher, eventDelegator)
        {
            unityObjectSource = source;
        }

        public WalletConnectUnitySession(ClientMeta clientMeta, WalletConnect source, string bridgeUrl = null, ITransport transport = null, ICipher cipher = null, int chainId = 1, EventDelegator eventDelegator = null) : base(clientMeta, bridgeUrl, transport, cipher, chainId, eventDelegator)
        {
            unityObjectSource = source;
        }

        internal string KeyData => _key;
    }
}