using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Services.IServices;
using System.Linq.Expressions;

namespace FinancialAssets.WebApp.Services
{
    public class FullReportBuilder : IReportBuilder
    {
        public async Task<ResponseDto> Build(IEnumerable<Asset> listAssets)
        {
            try
            {
                var assetsReport = await GetAssetsReport(listAssets);

                var fullReport = new FullReport
                {
                    Assets = assetsReport.ToList(),
                    TotalSpent = assetsReport.Sum(row => row.Spent),
                    TotalSoldOn = assetsReport.Sum(row => row.SoldOn),
                    //ProfitPercent = ,
                };

                return new ResponseDto
                {
                    IsSuccess = true,
                    Result = fullReport,
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    DisplayMessage = "Не удалось построить отчет",
                    ErrorMessages = ex.Message
                };
            }
        }

        private async Task<IEnumerable<AssetReport>> GetAssetsReport(IEnumerable<Asset> listAssets)
        {
            var listAssetsReport = new List<AssetReport>();

            foreach(var assetGroup in listAssets.GroupBy(asset => asset.Name))
            {
                var assetReport = new AssetReport{ Name = assetGroup.Key };

                foreach(var asset in assetGroup)
                {
                    if (asset.Count <= 0)
                        continue;

                    if (asset.Operation == "Buy")                   //todo Сделать глобальный enum
                    {
                        assetReport.Spent += asset.Price * asset.Count;
                        assetReport.Count += asset.Count;
                    }
                    else if (asset.Operation == "Sale")
                    {
                        assetReport.SoldOn += asset.Price * asset.Count;
                        assetReport.Count -= asset.Count;
                    }
                }

                assetReport.AvgPrice = (assetReport.Spent + assetReport.SoldOn) / assetReport.Count;

                listAssetsReport.Add(assetReport);
            }
            
            return listAssetsReport;
        }
    }
}
