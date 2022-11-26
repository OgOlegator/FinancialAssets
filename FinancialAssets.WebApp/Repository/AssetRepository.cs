using FinancialAssets.WebApp.DbContexts;
using FinancialAssets.WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssets.WebApp.Repository
{
    public class AssetRepository : IAssetRepository
    {
        private readonly ApplicationDbContext _db;

        public AssetRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsset(Asset asset)
        {
            _db.Assets.Add(asset);

            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsset(int id)
        {
            var asset = await _db.Assets.FirstOrDefaultAsync(asset => asset.Id == id);

            if (asset == null)
                return false;

            _db.Assets.Remove(asset);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Asset>> GetAssets()
        {
            var listAssets = await _db.Assets.ToListAsync();
            return listAssets;
        }

        public async Task<IEnumerable<Asset>> GetAssetsByName(string name)
        {
            var listAssets = await _db.Assets.Where(asset => asset.Name.Contains(name)).ToListAsync();
            return listAssets;
        }

        public async Task<Asset> GetAssetById(int id)
        {
            var asset = await _db.Assets.FirstOrDefaultAsync(asset => asset.Id == id);

            return asset;
        }
    }
}
