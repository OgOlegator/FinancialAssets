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
            using (var transactionDb = _dbContext.Database.BeginTransaction())
            {
                var errorAssets = new List<Asset>();

                foreach (var asset in (List<Asset>)data)
                {
                    try
                    {
                        await _assetRepository.AddAsset(asset);
                    }
                    catch 
                    {
                        errorAssets.Add(asset);

                        continue;
                    }
                }

                if(errorAssets.Count > 0)
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        DisplayMessage = "Не удалось загрузить файл. Ошибка в данных",
                        Result = errorAssets
                    };

                Task.WaitAll();
                await transactionDb.CommitAsync();
            }
            
            return new ResponseDto
            {
                IsSuccess = true,
                DisplayMessage = "Загрузка выполнена успешно"
            };
        }
    }
}
