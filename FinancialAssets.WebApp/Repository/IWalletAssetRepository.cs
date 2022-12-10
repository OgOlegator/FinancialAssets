using FinancialAssets.WebApp.Models;

namespace FinancialAssets.WebApp.Repository
{
    public interface IWalletAssetRepository
    {

        Task<IEnumerable<Asset>> GetWaletAsset();

        Task<IEnumerable<Asset>> GetWalletAssetByName(string name);

        Task<Asset> GetWalletAssetById(int id);

        Task AddWaletAsset(Asset asset);

        Task<bool> DeleteWaletAsset(int Id);

    }
}
