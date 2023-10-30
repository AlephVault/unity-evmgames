using NBitcoin;

namespace AlephVault.Unity.EVMGames.Vendor.Nethereum.HdWallet
{
    public class SecureRandom : IRandom
    {
        public Org.BouncyCastle.Security.SecureRandom SecureRandomInstance =
            new Org.BouncyCastle.Security.SecureRandom();

        public SecureRandom(Org.BouncyCastle.Security.SecureRandom secureRandom = null)
        {
            if(secureRandom != null)
            {
                SecureRandomInstance = secureRandom;
            }
        }

        public void GetBytes(byte[] output)
        {
            SecureRandomInstance.NextBytes(output);
        }
    }
}
