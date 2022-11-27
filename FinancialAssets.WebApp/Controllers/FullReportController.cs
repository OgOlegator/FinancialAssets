using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Services;
using FinancialAssets.WebApp.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssets.WebApp.Controllers
{
    public class FullReportController : Controller
    {
        private readonly IReportBuilder _builder;
        private readonly IAssetRepository _repository;
        private FullReport _report;

        public FullReportController(IReportBuilder builder, IAssetRepository repository)
        {
            _builder = builder;
            _repository = repository;
        }

        public IActionResult ReportIndex()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetReport()
        {
            var listAssets = await _repository.GetAssets();

            var response = await _builder.Build(listAssets);

            if(response == null || !response.IsSuccess)
                return RedirectToAction(nameof(ReportIndex));

            _report = (FullReport) response.Result;

            return View(response.Result);
        }
    }
}
