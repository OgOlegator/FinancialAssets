using FinancialAssets.WebApp.DbContexts;
using FinancialAssets.WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssets.WebApp.Repository
{
    public class WalletAssetRepository : IWalletAssetRepository
    {
        private readonly ApplicationDbContext _db;

        public WalletAssetRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<WalletAsset> AddWaletAsset(WalletAsset asset)
        {
            var assetInDb = await _db.WaletAssets.FirstOrDefaultAsync(item => item.Coin == asset.Coin && item.Wallet == asset.Wallet);

            if (assetInDb == null)
                _db.WaletAssets.Add(asset);
            else
            {
                throw new DuplicateWaitObjectException("Такая запись уже существует");
            }

            await _db.SaveChangesAsync();

            return asset;
        }

        public async Task<WalletAsset> ChangeWalletAsset(WalletAsset asset)
        {
            var assetInDb = await _db.WaletAssets.FirstOrDefaultAsync(item => item.Coin == asset.Coin && item.Wallet == asset.Wallet);

            if (assetInDb == null)
                throw new ArgumentNullException("Запись актива не найдена");
            else
            {
                _db.WaletAssets.Update(asset);
            }

            await _db.SaveChangesAsync();

            return asset;
        }

        public async Task<bool> DeleteWaletAsset(string coin, string wallet)
        {
            var walletAsset = await _db.WaletAssets.FirstOrDefaultAsync(asset => asset.Coin == coin && asset.Wallet == wallet);

            if (walletAsset == null)
                return false;

            _db.WaletAssets.Remove(walletAsset);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<WalletAsset>> GetWaletAssets()
        {
            var listAssets = await _db.WaletAssets.ToListAsync();
            return listAssets;
        }

        public async Task<WalletAsset> GetWalletAssetByKey(string coin, string wallet)
        {
            var asset = await _db.WaletAssets.FirstOrDefaultAsync(asset => asset.Coin == coin && asset.Wallet == wallet);

            return asset;
        }

        public async Task<IEnumerable<WalletAsset>> GetWalletAssetsByCoin(string coin)
        {
            var listAssets = await _db.WaletAssets.Where(asset => asset.Coin.Contains(coin)).ToListAsync();
            return listAssets;
        }

        public async Task<IEnumerable<WalletAsset>> GetWalletAssetsByWallet(string wallet)
        {
            var listAssets = await _db.WaletAssets.Where(asset => asset.Wallet.Contains(wallet)).ToListAsync();
            return listAssets;
        }
    }
}
