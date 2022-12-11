using FinancialAssets.WebApp.Models;

namespace FinancialAssets.WebApp.Services.IServices
{
    public interface ICacheReport
    {

        FullReport GetLastReport();

        void SetLastReport(FullReport report);

    }
}
