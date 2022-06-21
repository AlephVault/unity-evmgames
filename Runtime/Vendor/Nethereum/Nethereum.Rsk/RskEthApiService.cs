﻿using AlephVault.Unity.EVMGames.Nethereum.JsonRpc.Client;
using AlephVault.Unity.EVMGames.Nethereum.RPC;
using AlephVault.Unity.EVMGames.Nethereum.Rsk.RPC.RskEth;

namespace AlephVault.Unity.EVMGames.Nethereum.Rsk
{
    public class RskEthApiService : RpcClientWrapper, IRskEthApiService
    {
        public RskEthApiService(IClient client) : base(client)
        {
            GetBlockWithTransactionsByHash = new RskEthGetBlockWithTransactionsByHash(client);
            GetBlockWithTransactionsByNumber = new RskEthGetBlockWithTransactionsByNumber(client);
            GetBlockWithTransactionsHashesByHash = new RskEthGetBlockWithTransactionsHashesByHash(client);
            GetBlockWithTransactionsHashesByNumber = new RskEthGetBlockWithTransactionsHashesByNumber(client);
        }
        public IRskEthGetBlockWithTransactionsByHash GetBlockWithTransactionsByHash { get; private set; }
        public IRskEthGetBlockWithTransactionsByNumber GetBlockWithTransactionsByNumber { get; private set; }
        public IRskEthGetBlockWithTransactionsHashesByHash GetBlockWithTransactionsHashesByHash { get; private set; }
        public IRskEthGetBlockWithTransactionsHashesByNumber GetBlockWithTransactionsHashesByNumber { get; private set; }
    }
}