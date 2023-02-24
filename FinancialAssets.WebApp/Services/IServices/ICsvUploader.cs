using FinancialAssets.WebApp.Models.Dtos;

namespace FinancialAssets.WebApp.Services.IServices
{
    public interface ICsvUploader
    {

        public Task<ResponseDto> Parse(IFormFile uploadedFile);

        public Task<ResponseDto> Upload(object data);

    }
}
