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
        private readonly ICacheReport _cacheReport;

        public FullReportController(IReportBuilder builder, IAssetRepository repository, ICacheReport cacheReport)
        {
            _builder = builder;
            _repository = repository;
            _cacheReport = cacheReport;
        }

        public IActionResult ReportIndex()
        {
            return View(_cacheReport.GetLastReport());
        }

        [HttpGet]
        public async Task<IActionResult> GetReport()
        {
            var response = await _builder.Build();

            if(response == null || !response.IsSuccess)
                return RedirectToAction(nameof(ReportIndex));

            _cacheReport.SetLastReport((FullReport) response.Result);

            return RedirectToAction(nameof(ReportIndex));
        }
    }
}
