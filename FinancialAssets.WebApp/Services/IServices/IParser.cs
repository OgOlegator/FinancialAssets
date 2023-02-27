using FinancialAssets.WebApp.Models.Dtos;

namespace FinancialAssets.WebApp.Services.IServices
{
    public interface IParser
    {

        public Task<ResponseDto> Parse(IFormFile uploadedFile);

    }
}
