using FinancialAssets.WebApp.Models;

namespace FinancialAssets.WebApp.Services.IServices
{
    public interface IReportBuilder
    {

        Task<ResponseDto> Build();

    }
}
