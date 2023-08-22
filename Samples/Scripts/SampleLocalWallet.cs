using AlephVault.Unity.EVMGames.Utils.Wallets;
using NBitcoin;
using Nethereum.Web3;
using UnityEngine;


namespace AlephVault.Unity.EVMGames
{
    using Vendor.Nethereum.HdWallet;

    namespace Samples
    {
        public class SampleLocalWallet : MonoBehaviour
        {
            // Start is called before the first frame update
            private async void Start()
            {
                // Generate the mnemonics.
                Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.TwentyFour);
                Debug.Log($"Mnemonic is: {mnemonic}");
                
                // Generate a wallet's account.
                Wallet w = new Wallet(mnemonic.ToString(), "Password1");
                Web3 wallet1acc0 = await LocalWallet.GetWeb3(mnemonic.ToString(), "Password1");
                Debug.Log($"First Web3's address: {wallet1acc0.TransactionManager.Account.Address}");
                
                // Generate a wallet's account, again.
                Web3 wallet1acc1 = await LocalWallet.GetWeb3(mnemonic.ToString(), "Password2");
                Debug.Log($"Second Web3's address: {wallet1acc1.TransactionManager.Account.Address}");
            }
        }
    }
}
