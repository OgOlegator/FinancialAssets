using FinancialAssets.WebApp.Models;

namespace FinancialAssets.WebApp.Repository
{
    public interface IWalletAssetRepository
    {

        Task<IEnumerable<WalletAsset>> GetWaletAssets();

        Task<WalletAsset> GetWalletAssetByKey(string coin, string wallet);

        Task<IEnumerable<WalletAsset>> GetWalletAssetsByCoin(string coin);

        Task<IEnumerable<WalletAsset>> GetWalletAssetsByWallet(string wallet);

        Task<WalletAsset> AddWaletAsset(WalletAsset asset);

        Task<bool> DeleteWaletAsset(string coin, string wallet);

        Task<WalletAsset> ChangeWalletAsset(WalletAsset asset);
    }
}
