﻿using AlephVault.Unity.EVMGames.Nethereum.Geth.RPC.Admin;

namespace AlephVault.Unity.EVMGames.Nethereum.Geth
{
    public interface IAdminApiService
    {
        IAdminAddPeer AddPeer { get; }
        IAdminDatadir Datadir { get; }
        IAdminNodeInfo NodeInfo { get; }
        IAdminPeers Peers { get; }
        IAdminStartRPC StartRPC { get; }
        IAdminStartWS StartWS { get; }
        IAdminStopRPC StopRPC { get; }
        IAdminStopWS StopWS { get; }
    }
}