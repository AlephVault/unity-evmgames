using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Events;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Events.Request;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Models;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Models.Ethereum;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Network;
using AlephVault.Unity.EVMGames.WalletConnectSharp.Core.Utils;

namespace AlephVault.Unity.EVMGames.WalletConnectSharp.Core
{
    public class WalletConnectSession : WalletConnectProtocol
    {
        
        private string _handshakeTopic;
        private long _handshakeId;
        private string _clientId = "";

        public event EventHandler<WalletConnectSession> OnSessionConnect;
        public event EventHandler<WalletConnectSession> OnSessionCreated;
        public event EventHandler<WalletConnectSession> OnSessionResumed;
        public event EventHandler OnSessionDisconnect;
        public event EventHandler<WalletConnectSession> OnSend;
        public event EventHandler<WCSessionData> SessionUpdate;

        public int NetworkId { get; private set; }
        
        public string[] Accounts { get; private set; }
        
        public bool ReadyForUserPrompt { get; private set; }
        
        public bool SessionUsed { get; private set; }

        public int ChainId { get; private set; }
        
        public string URI
        {
            get
            {
                var topicEncode = WebUtility.UrlEncode(_handshakeTopic);
                var versionEncode = WebUtility.UrlEncode(Version);
                var bridgeUrlEncode = WebUtility.UrlEncode(_bridgeUrl);
                var keyEncoded = WebUtility.UrlEncode(_key);

                return "wc:" + topicEncode + "@" + versionEncode + "?bridge=" + bridgeUrlEncode + "&key=" + keyEncoded;
            }
        }
        
        public WalletConnectSession(SavedSession savedSession, ITransport transport = null, ICipher cipher = null, EventDelegator eventDelegator = null) : base(savedSession, transport, cipher, eventDelegator)
        {
            DappMetadata = savedSession.DappMeta;
            WalletMetadata = savedSession.WalletMeta;
            ChainId = savedSession.ChainID;
            _clientId = savedSession.ClientID;
            Accounts = savedSession.Accounts;
            NetworkId = savedSession.NetworkID;
            _handshakeId = savedSession.HandshakeID;
            SessionConnected = true;
        }

        public WalletConnectSession(ClientMeta clientMeta, string bridgeUrl = null, ITransport transport = null, ICipher cipher = null, int chainId = 1, EventDelegator eventDelegator = null) : base(transport, cipher, eventDelegator)
        {
            if (clientMeta == null)
            {
                throw new ArgumentException("clientMeta cannot be null!");
            }

            if (string.IsNullOrWhiteSpace(clientMeta.Description))
            {
                throw new ArgumentException("clientMeta must include a valid Description");
            }
            
            if (string.IsNullOrWhiteSpace(clientMeta.Name))
            {
                throw new ArgumentException("clientMeta must include a valid Name");
            }
            
            if (string.IsNullOrWhiteSpace(clientMeta.URL))
            {
                throw new ArgumentException("clientMeta must include a valid URL");
            }
            
            if (clientMeta.Icons == null || clientMeta.Icons.Length == 0)
            {
                throw new ArgumentException("clientMeta must include an array of Icons the Wallet app can use. These Icons must be URLs to images. You must include at least one image URL to use");
            }
            
            if (bridgeUrl == null)
            {
                bridgeUrl = DefaultBridge.ChooseRandomBridge();
            }

            bridgeUrl = DefaultBridge.GetBridgeUrl(bridgeUrl);
            
            if (bridgeUrl.StartsWith("https"))
                bridgeUrl = bridgeUrl.Replace("https", "wss");
            else if (bridgeUrl.StartsWith("http"))
                bridgeUrl = bridgeUrl.Replace("http", "ws");
            
            DappMetadata = clientMeta;
            ChainId = chainId;
            _bridgeUrl = DefaultBridge.GetBridgeUrl(bridgeUrl);
            SessionConnected = false;
            
            _handshakeTopic = Guid.NewGuid().ToString();
            _clientId = Guid.NewGuid().ToString();

            GenerateKey();
            Transport?.ClearSubscriptions();
            SessionUsed = false;
            ReadyForUserPrompt = false;
        }
        
        private void EnsureNotDisconnected()
        {
            if (Disconnected)
            {
                throw new IOException(
                    "Session stale! The session has been disconnected. This session cannot be reused.");
            }
        }
        
        private void GenerateKey()
        {
            //Generate a random secret
            byte[] secret = new byte[32];
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            rngCsp.GetBytes(secret);

            _keyRaw = secret;

            //Convert hex 
            _key = _keyRaw.ToHex().ToLower();
        }

        protected virtual async Task<WCSessionData> ConnectSession()
        {
            Connecting = true;
            try
            {
                if (SessionConnected)
                {
                    //Listen for the _handshakeId response
                    //The response will be of type WCSessionRequestResponse
                    //We do this now before subscribing
                    //This is in case we need to respond to a session disconnect and this is a
                    //resume session
                    Events.ListenForResponse<WCSessionRequestResponse>(_handshakeId, HandleSessionResponse);
                }
                
                if (!TransportConnected)
                {
                    await SetupTransport();
                }

                ReadyForUserPrompt = false;
                await SubscribeAndListenToTopic(_clientId);

                ListenToTopic(_handshakeTopic);

                WCSessionData result;
                if (!SessionConnected)
                {
                    result = await CreateSession();
                    //Reset this back after we have established a session
                    ReadyForUserPrompt = false;
                    Connecting = false;

                    OnSessionCreated?.Invoke(this, this);
                }
                else
                {
                    result = new WCSessionData()
                    {
                        accounts = Accounts,
                        approved = true,
                        chainId = ChainId,
                        networkId = NetworkId,
                        peerId = PeerId,
                        peerMeta = WalletMetadata
                    };
                    Connecting = false;

                    OnSessionResumed?.Invoke(this, this);
                }

                OnSessionConnect?.Invoke(this, this);

                return result;
            }
            catch (IOException e)
            {
                //If the transport is connected, then disconnect that
                //we tried our best, they can try again
                if (TransportConnected)
                {
                    await DisconnectTransport();
                }
                else
                {
                    throw new IOException("Transport Connection failed", e);
                }

                throw new IOException("Session Connection failed", e);
            }
            finally
            {
                //The session has been made, we are no longer ready for another user prompt
                ReadyForUserPrompt = false;
                Connecting = false;
            }
        }
        
        public override async Task<WCSessionData> Connect()
        {
            EnsureNotDisconnected();
            return await ConnectSession();
        }

        public async Task DisconnectSession(string disconnectMessage = "Session Disconnected")
        {
            EnsureNotDisconnected();
            var request = new WCSessionUpdate(new WCSessionData()
            {
                approved = false,
                chainId = 0,
                accounts = null,
                networkId = 0
            });

            await SendRequest(request);
            await DisconnectTransport();
            HandleSessionDisconnect(disconnectMessage, "disconnect");
        }

        public override async Task Disconnect()
        {
            await DisconnectSession();
        }
        
        public async Task<string> EthSign(string address, string message)
        {
            EnsureNotDisconnected();
            
            if (!message.IsHex())
            {
                var rawMessage = Encoding.UTF8.GetBytes(message);
                
                var byteList = new List<byte>();
                var bytePrefix = "0x19".HexToByteArray();
                var textBytePrefix = Encoding.UTF8.GetBytes("Ethereum Signed Message:\n" + rawMessage.Length);

                byteList.AddRange(bytePrefix);
                byteList.AddRange(textBytePrefix);
                byteList.AddRange(rawMessage);
                
                var hash = new Sha3Keccack().CalculateHash(byteList.ToArray());

                message = "0x" + hash.ToHex();
            }

            var request = new EthSign(address, message);
            var response = await Send<EthSign, EthResponse>(request);
            return response.Result;
        }

        public async Task<string> EthPersonalSign(string address, string message)
        {
            EnsureNotDisconnected();
            
            if (!message.IsHex())
            {
                /*var rawMessage = Encoding.UTF8.GetBytes(message);
                
                var byteList = new List<byte>();
                var bytePrefix = "0x19".HexToByteArray();
                var textBytePrefix = Encoding.UTF8.GetBytes("Ethereum Signed Message:\n" + rawMessage.Length);

                byteList.AddRange(bytePrefix);
                byteList.AddRange(textBytePrefix);
                byteList.AddRange(rawMessage);
                
                var hash = new Sha3Keccack().CalculateHash(byteList.ToArray());*/

                message = "0x" + Encoding.UTF8.GetBytes(message).ToHex();
            }
            
            var request = new EthPersonalSign(address, message);
            var response = await Send<EthPersonalSign, EthResponse>(request);
            return response.Result;
        }

        public async Task<string> EthSignTypedData<T>(string address, T data, EIP712Domain eip712Domain)
        {
            EnsureNotDisconnected();
            
            var request = new EthSignTypedData<T>(address, data, eip712Domain);

            var response = await Send<EthSignTypedData<T>, EthResponse>(request);

            return response.Result;
        }

        public async Task<string> EthSendTransaction(params TransactionData[] transaction)
        {
            EnsureNotDisconnected();
            
            var request = new EthSendTransaction(transaction);
            
            var response = await Send<EthSendTransaction, EthResponse>(request);

            return response.Result;
        }

        public async Task<string> EthSignTransaction(params TransactionData[] transaction)
        {
            EnsureNotDisconnected();
            
            var request = new EthSignTransaction(transaction);
            
            var response = await Send<EthSignTransaction, EthResponse>(request);

            return response.Result;
        }
        
        
        public async Task<string> EthSendRawTransaction(string data, Encoding messageEncoding = null)
        {
            EnsureNotDisconnected();
            
            if (!data.IsHex())
            {
                var encoding = messageEncoding;
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                
                data = "0x" + encoding.GetBytes(data).ToHex();
            }
            
            var request = new EthGenericRequest<string>("eth_sendRawTransaction", data);
            
            var response = await Send<EthGenericRequest<string>, EthResponse>(request);

            return response.Result;
        }

        public async Task<R> Send<T, R>(T data) where T : JsonRpcRequest where R : JsonRpcResponse
        {
            EnsureNotDisconnected();
            
            TaskCompletionSource<R> eventCompleted = new TaskCompletionSource<R>(TaskCreationOptions.None);
            
            Events.ListenForResponse<R>(data.ID, (sender, @event) =>
            {
                var response = @event.Response;
                if (response.IsError)
                {
                    eventCompleted.SetException(new IOException(response.Error.Message));
                }
                else
                {
                    eventCompleted.SetResult(@event.Response);
                }
                
            });

            await SendRequest(data);

            OnSend?.Invoke(this, this);

            return await eventCompleted.Task;
        }

        /// <summary>
        /// Create a new WalletConnect session with a Wallet.
        /// </summary>
        /// <returns></returns>
        private async Task<WCSessionData> CreateSession()
        {
            EnsureNotDisconnected();
            
            var data = new WcSessionRequest(DappMetadata, _clientId, ChainId);

            _handshakeId = data.ID;

            //Debug.Log("[WalletConnect] Sending Session to topic " + _handshakeTopic);

            await SendRequest(data, _handshakeTopic);

            SessionUsed = true;

            TaskCompletionSource<WCSessionData> eventCompleted =
                new TaskCompletionSource<WCSessionData>(TaskCreationOptions.None);

            //Listen for the _handshakeId response
            //The response will be of type WCSessionRequestResponse
            Events.ListenForResponse<WCSessionRequestResponse>(this._handshakeId, HandleSessionResponse);
            
            //Listen for wc_sessionUpdate requests
            Events.ListenFor("wc_sessionUpdate",
                (object sender, GenericEvent<WCSessionUpdate> @event) =>
                    HandleSessionUpdate(@event.Response.parameters[0]));

            //Listen for the "connect" event triggered by 'HandleSessionResponse' above
            //This will have the type WCSessionData
            Events.ListenFor<WCSessionData>("connect",
                (sender, @event) =>
                {
                    eventCompleted.TrySetResult(@event.Response);
                });
            
            //Listen for the "session_failed" event triggered by 'HandleSessionResponse' above
            //This will have the type failure reason
            Events.ListenFor<ErrorResponse>("session_failed",
                delegate(object sender, GenericEvent<ErrorResponse> @event)
                {
                    if (@event.Response.Message == "Not Approved" || @event.Response.Message == "Session Rejected")
                    {
                        eventCompleted.TrySetCanceled();
                    }
                    else
                    {
                        eventCompleted.TrySetException(
                            new IOException("WalletConnect: Session Failed: " + @event.Response.Message));
                    }
                });
            
            ReadyForUserPrompt = true;
            
            //Debug.Log("[WalletConnect] Session Ready for Wallet");

            var response = await eventCompleted.Task;

            ReadyForUserPrompt = false;

            return response;
        }

        private void HandleSessionResponse(object sender, JsonRpcResponseEvent<WCSessionRequestResponse> jsonresponse)
        {
            var response = jsonresponse.Response.result;

            if (response != null && response.approved)
            {
                HandleSessionUpdate(response);
            }
            else if (jsonresponse.Response.IsError)
            {
                HandleSessionDisconnect(jsonresponse.Response.Error.Message, "session_failed");
            }
            else
            {
                HandleSessionDisconnect("Not Approved", "session_failed");
            }
        }

        private void HandleSessionUpdate(WCSessionData data)
        {
            if (data == null) return;

            bool wasConnected = SessionConnected;

            //We are connected if we are approved
            SessionConnected = data.approved;
            
            if (data.chainId != null)
                ChainId = (int)data.chainId;
            
            if (data.networkId != null)
                NetworkId = (int)data.networkId;

            Accounts = data.accounts;

            if (!wasConnected)
            {
                PeerId = data.peerId;

                WalletMetadata = data.peerMeta;

                Events.Trigger("connect", data);
            }
            else if (wasConnected && !SessionConnected)
            {
                HandleSessionDisconnect("Wallet Disconnected");
            }
            else
            {
                Events.Trigger("session_update", data);
            }

            SessionUpdate?.Invoke(this, data);
        }

        private void HandleSessionDisconnect(string msg, string topic = "disconnect")
        {
            SessionConnected = false;
            Disconnected = true;

            Events.Trigger(topic, new ErrorResponse(msg));

            if (TransportConnected)
            {
                DisconnectTransport();
            }
            
            _activeTopics.Clear();
            
            Events.Clear();

            OnSessionDisconnect?.Invoke(this, EventArgs.Empty);
        }
        
        
        
        /// <summary>
        /// Creates and returns a serializable class that holds all session data required to resume later
        /// </summary>
        /// <returns></returns>
        public SavedSession SaveSession()
        {
            if (!SessionConnected || Disconnected)
            {
                return null;
            }
            
            return new SavedSession(_clientId, _handshakeId, _bridgeUrl, _key, _keyRaw, PeerId, NetworkId, Accounts, ChainId, DappMetadata, WalletMetadata);
        }

        /// <summary>
        /// Save the current session to a Stream. This function will write a GZIP Compressed JSON blob
        /// of the contents of SaveSession()
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="leaveStreamOpen">Whether to leave the stream open</param>
        /// <exception cref="IOException">If there is currently no session active, or if writing to the stream fails</exception>
        public void SaveSession(Stream stream, bool leaveStreamOpen = true)
        {
            //We'll save the current session as a GZIP compressed JSON blob
            var data = SaveSession();

            if (data == null)
            {
                throw new IOException("No session is active to save");
            }

            var json = JsonConvert.SerializeObject(data);

            byte[] encodedJson = Encoding.UTF8.GetBytes(json);
            
           using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress, leaveStreamOpen))
            {
                byte[] sizeEncoded = BitConverter.GetBytes(encodedJson.Length);
                
                gZipStream.Write(sizeEncoded, 0, sizeEncoded.Length);
                gZipStream.Write(encodedJson, 0, encodedJson.Length);
            }
        }

        /// <summary>
        /// Reads a GZIP Compressed JSON blob of a SavedSession object from a given Stream. This is the reverse of
        /// SaveSession(Stream)
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="leaveStreamOpen">Whether to leave the stream open</param>
        /// <exception cref="IOException">If reading from the stream fails</exception>
        /// <returns>A SavedSession object</returns>
        public static SavedSession ReadSession(Stream stream, bool leaveStreamOpen = true)
        {
            string json;
           using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress, leaveStreamOpen))
            {
                byte[] sizeEncoded = new byte[4];

                gZipStream.Read(sizeEncoded, 0, 4);

                int size = BitConverter.ToInt32(sizeEncoded, 0);

                byte[] jsonEncoded = new byte[size];

                gZipStream.Read(jsonEncoded, 0, size);

                json = Encoding.UTF8.GetString(jsonEncoded);
            }

            return JsonConvert.DeserializeObject<SavedSession>(json);
        }
    }
}