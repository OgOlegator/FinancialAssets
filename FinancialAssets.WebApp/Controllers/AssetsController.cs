using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using FinancialAssets.WebApp.Services.IServices;

namespace FinancialAssets.WebApp.Controllers
{
    public class AssetsController : Controller
    {
        private readonly IAssetRepository _repository;
        private readonly ICsvUploader _uploader;

        public AssetsController(IAssetRepository repository, ICsvUploader uploader)
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

        public async Task<IActionResult> UploadAssets(UploadAssetsViewModel? model = null)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAssets(IFormFile uploadedFile)
        {
            var response = await _uploader.Parse(uploadedFile);

            var viewModel = new UploadAssetsViewModel
            {
                IsLoaded = true,
                Assets = (List<Asset>) response.Result,
                Message = response.DisplayMessage
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAssets(UploadAssetsViewModel model)
        {
            var response = await _uploader.Upload(model.Assets);

            return RedirectToAction(nameof(AssetsIndex));
        }
    }
}
