using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Services.IServices;

namespace FinancialAssets.WebApp.Services
{
    public class FullReportBuilder : IReportBuilder
    {
        private readonly IAssetRepository _repository;

        public FullReportBuilder(IAssetRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseDto> Build()
        {
            var report = new FullReport();         
            
            

            return null;
        }

        private async Task<IEnumerable<AssetReport>> GetAssetsReport()
        {
            var listAssets = await _repository.GetAssets();

            var assetsReport = new List<AssetReport>();

            foreach(var assetGroup in listAssets.GroupBy(asset => asset.Name))
            {
                var assetReport = new AssetReport
                {
                    Name = assetGroup.Key,
                     
                };
            }
            

            return null;
        }
    }
}
