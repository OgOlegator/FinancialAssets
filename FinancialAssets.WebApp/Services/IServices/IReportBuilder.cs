using FinancialAssets.WebApp.Models.Dtos;

namespace FinancialAssets.WebApp.Services.IServices
{
    public interface IReportBuilder
    {

        Task<ResponseDto> Build();

    }
}
