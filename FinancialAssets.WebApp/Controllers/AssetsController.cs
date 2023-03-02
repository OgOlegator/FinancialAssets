using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using FinancialAssets.WebApp.Services.IServices;
using Azure;

namespace FinancialAssets.WebApp.Controllers
{
    public class AssetsController : Controller
    {
        private readonly IAssetRepository _repository;
        private readonly IParser _parser;
        private readonly IUploader _uploader;

        public AssetsController(IAssetRepository repository, IParser parser, IUploader uploader)
        {
            _repository = repository;
            _parser = parser;
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

        public IActionResult Upload(UploadViewModel model = null)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Parse(IFormFile uploadedFile)
        {
            try
            {
                var response = await _parser.Parse(uploadedFile);

                if(!response.IsSuccess)
                {
                    return RedirectToAction(nameof(Upload), new UploadViewModel { Message = response.DisplayMessage });
                }

                var viewModel = new ParseViewModel
                {
                    Assets = (List<Asset>)response.Result,
                    Message = response.DisplayMessage
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Upload), new UploadViewModel { Message = "Ошибка при чтении файла" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(ParseViewModel model)
        {
            var response = await _uploader.Upload(model.Assets);

            return View(new SaveViewModel
            {
                Message = response.DisplayMessage,
                IsSave  = response.IsSuccess,
                ErrorAssets = (List<Asset>) response.Result
            });
        }
    }
}
