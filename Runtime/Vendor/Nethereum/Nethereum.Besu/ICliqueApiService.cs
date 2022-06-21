using AlephVault.Unity.EVMGames.Nethereum.Besu.RPC.Clique;

namespace AlephVault.Unity.EVMGames.Nethereum.Besu
{
    public interface ICliqueApiService
    {
        ICliqueDiscard Discard { get; }
        ICliqueGetSigners GetSigners { get; }
        ICliqueGetSignersAtHash GetSignersAtHash { get; }
        ICliqueProposals Proposals { get; }
        ICliquePropose Propose { get; }
    }
}