using FinancialAssets.WebApp.Models;

namespace FinancialAssets.WebApp.Repository
{
    public interface IAssetRepository
    {

        Task<IEnumerable<Asset>> GetAssets();

        Task<IEnumerable<Asset>> GetAssetsByName(string name);

        Task<Asset> GetAssetById(int id);

        Task AddAsset(Asset asset);

        Task<bool> DeleteAsset(int Id);

    }
}
