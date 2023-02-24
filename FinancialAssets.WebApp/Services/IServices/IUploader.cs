namespace FinancialAssets.WebApp.Services.IServices
{
    public interface IUploader
    {

        public Task<bool> UploadAsync(IFormFile uploadedFile);

    }
}
