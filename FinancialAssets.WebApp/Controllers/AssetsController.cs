using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using FinancialAssets.WebApp.Services.IServices;

namespace FinancialAssets.WebApp.Controllers
{
    public class AssetsController : Controller
    {
        private readonly IAssetRepository _repository;
        private readonly IUploader _uploader;

        public AssetsController(IAssetRepository repository, IUploader uploader)
        {
            _repository = repository;
            _uploader = uploader;
        }

        public async Task<IActionResult> AssetsIndex(string searchAsset)
        {
            var listAssets = await _repository.GetAssets();

            var assetForView = listAssets
                .Where(asset => searchAsset == null || asset.Name.Contains(searchAsset));

            return View(new AssetViewModel
            {
                Assets = assetForView,
                SearchCoin = searchAsset,
            });
        }

        public async Task<IActionResult> AddAsset()
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAsset(Asset model)
        {
            if (ModelState.IsValid)
                await _repository.AddAsset(model);

            return View(model);
        }

        public async Task<IActionResult> AssetDelete(int id)
        {
            var asset = await _repository.GetAssetById(id);

            if (asset != null)
            {
                var result = await _repository.DeleteAsset(asset.Id);

                if(result)
                    return RedirectToAction(nameof(AssetsIndex));
            }

            return NotFound();
        }

        public async Task<IActionResult> UploadAssets()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAssets(IFormFile uploadedFile)
        {
            await _uploader.UploadAsync(uploadedFile);

            return RedirectToAction(nameof(AssetsIndex));
        }
    }
}
