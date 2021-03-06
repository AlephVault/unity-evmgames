namespace AlephVault.Unity.EVMGames.Nethereum.Hex.HexConvertors
{
    public interface IHexConvertor<T>
    {
        string ConvertToHex(T value);
        T ConvertFromHex(string value);
    }
}