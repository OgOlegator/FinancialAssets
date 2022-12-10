using FinancialAssets.WebApp.Models;

namespace FinancialAssets.WebApp.Repository
{
    public class WalletAssetRepository : IWalletAssetRepository
    {
        public Task AddWaletAsset(Asset asset)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteWaletAsset(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Asset>> GetWaletAsset()
        {
            throw new NotImplementedException();
        }

        public Task<Asset> GetWalletAssetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Asset>> GetWalletAssetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
