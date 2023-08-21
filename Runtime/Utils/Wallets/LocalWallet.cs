using System.Numerics;
using System.Threading.Tasks;
using Nethereum.HdWallet;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;


namespace AlephVault.Unity.EVMGames
{
    namespace Utils
    {
        namespace Wallets
        {
            /// <summary>
            ///   A local wallet takes the mnemonic as a string, an eventual
            ///   password (which works well to make deniable wallets) and
            ///   the account index inside a wallet. The returned Web3 client
            ///   (which might use a default client with a particular gateway
            ///   or a custom client) is signing-enabled.
            /// </summary>
            public static class LocalWallet
            {
                private static Account MakeAccount(
                    string mnemonic, string password,
                    int accountIndex = 0, BigInteger? chainId = null
                )
                {
                    Wallet wallet = new Wallet(mnemonic, password);
                    return wallet.GetAccount(accountIndex, chainId);
                }
                
                public static async Task<Web3> GetWeb3(
                    string mnemonic, string password, string gateway,int accountIndex = 0,
                    BigInteger? chainId = null
                )
                {
                    return new Web3(MakeAccount(
                        mnemonic, password, accountIndex, chainId
                    ), gateway);
                }

                public static async Task<Web3> GetWeb3(
                    string mnemonic, string password, IClient client, int accountIndex = 0,
                    BigInteger? chainId = null
                )
                {
                    return new Web3(MakeAccount(
                        mnemonic, password, accountIndex, chainId
                    ), client);
                }
            }
        }
    }
}
