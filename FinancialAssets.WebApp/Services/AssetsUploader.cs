using FinancialAssets.WebApp.DbContexts;
using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Models.Dtos;
using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Services.IServices;

namespace FinancialAssets.WebApp.Services
{
    /// <summary>
    /// Загрузка нескольких записей Активов
    /// </summary>
    public class AssetsUploader : IUploader
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAssetRepository _assetRepository;

        public AssetsUploader(ApplicationDbContext dbContext, IAssetRepository assetRepository)
        {
            _dbContext = dbContext;
            _assetRepository = assetRepository;
        }

        public async Task<ResponseDto> Upload(object data)
        {
            try
            {
                using (var transactionDb = _dbContext.Database.BeginTransaction())
                {
                    foreach (var asset in (List<Asset>)data)
                        await _assetRepository.AddAsset(asset);

                    Task.WaitAll();

                    await transactionDb.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    ErrorMessages = ex.ToString(),
                    DisplayMessage = ex.Message
                };
            }

            return new ResponseDto
            {
                IsSuccess = true,
            };
        }
    }
}
