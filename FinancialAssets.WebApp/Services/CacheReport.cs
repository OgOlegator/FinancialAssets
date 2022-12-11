using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Services.IServices;

namespace FinancialAssets.WebApp.Services
{
    public class CacheReport : ICacheReport
    {
        private FullReport _lastReport = new FullReport();

        public FullReport GetLastReport()
        {
            return _lastReport;
        }

        public void SetLastReport(FullReport report)
        {
            _lastReport = report;
        }
    }
}
