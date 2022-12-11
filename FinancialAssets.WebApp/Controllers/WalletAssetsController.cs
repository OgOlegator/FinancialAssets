using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Repository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssets.WebApp.Controllers
{
    public class WalletAssetsController : Controller
    {
        private readonly IWalletAssetRepository _repository;
        
        public WalletAssetsController(IWalletAssetRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> WalletAssetIndex(string searchCoin, string searchWallet)
        {
            var listAssets = await _repository.GetWaletAssets();

            var assetsForView = listAssets
                .Where(asset => searchCoin == null || asset.Coin.Contains(searchCoin))
                .Where(asset => searchWallet == null || asset.Wallet.Contains(searchWallet));

            return View(assetsForView);
        }

        public async Task<IActionResult> WalletAssetDelete(string coin, string wallet)
        {
            var asset = await _repository.GetWalletAssetByKey(coin, wallet);

            if (asset != null)
            {
                var result = await _repository.DeleteWaletAsset(coin, wallet);

                if (result)
                    return RedirectToAction(nameof(WalletAssetIndex));
            }

            return NotFound();
        }

        public async Task<IActionResult> AddWalletAsset()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWalletAsset(WalletAsset model)
        {
            if (ModelState.IsValid)
                await _repository.AddWaletAsset(model);
            
            return View();
        }

        public async Task<IActionResult> WalletAssetChange(string coin, string wallet)
        {
            var changeAsset = await _repository.GetWalletAssetByKey(coin, wallet);

            if (changeAsset == null)
                NotFound();

            return View(changeAsset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WalletAssetChange(WalletAsset model)
        {
            if (ModelState.IsValid)
                await _repository.ChangeWalletAsset(model);

            return RedirectToAction(nameof(WalletAssetIndex));
        }
    }
}
