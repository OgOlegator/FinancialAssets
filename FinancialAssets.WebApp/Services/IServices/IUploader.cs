using FinancialAssets.WebApp.Models.Dtos;

namespace FinancialAssets.WebApp.Services.IServices
{
    public interface IUploader
    {

        public Task<ResponseDto> Upload(object data);

    }
}
