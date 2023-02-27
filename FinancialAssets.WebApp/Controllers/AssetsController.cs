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

        //todo Можно привести таблицу с ошибочными записями и указать их
        //На странице Upload надо обрабатывать ошибки при парсинге, выводить сообщение

        public IActionResult Upload(UploadViewModel? model = null)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(IFormFile uploadedFile)
        {
            try
            {
                var response = await _parser.Parse(uploadedFile);

                var viewModel = new SaveViewModel
                {
                    Assets = (List<Asset>)response.Result,
                    Message = response.DisplayMessage
                };

                ViewBag.Assets = response.Result;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return Upload(new UploadViewModel
                {
                    Message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(SaveViewModel model)
        {
            var response = await _uploader.Upload(ViewBag.Assets);

            return Upload(new UploadViewModel
            {
                Message = response.DisplayMessage,
            });
        }
    }
}
