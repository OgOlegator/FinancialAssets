using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Models.Dtos;
using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Services.IServices;
using NoobsMuc.Coinmarketcap.Client;
using System.Linq.Expressions;

namespace FinancialAssets.WebApp.Services
{
    public class FullReportBuilder : IReportBuilder
    {
        private const string apiKey = "CmcApiKey";
        private readonly IAssetRepository _repository;
        private readonly string _cmcApiKey;

        public FullReportBuilder(IAssetRepository repository, IConfiguration config)
        {
            _repository = repository;
            _cmcApiKey = config.GetValue<string>(apiKey);
        }

        public async Task<ResponseDto> Build()
        {
            try
            {
                var listAssets = await _repository.GetAssets();

                var assetsReport = await GetAssetsReport(listAssets);

                var fullReport = new FullReport
                {
                    Assets = assetsReport.ToList(),
                    TotalSpent = assetsReport.Sum(row => row.Spent),
                    TotalSoldOn = assetsReport.Sum(row => row.SoldOn),
                    //ProfitPercent = GetTotalProfit(assetsReport),
                };

                fullReport.AbsoluteProfit = GetAbsoluteProfit(
                    fullReport.Assets.Sum(asset => asset.AvgPrice * asset.Count),
                    fullReport.Assets.Sum(asset => asset.CurrentPrice * asset.Count));

                fullReport.ProfitPercent = GetProcentProfit(
                    fullReport.AbsoluteProfit,
                    fullReport.Assets.Sum(asset => asset.AvgPrice * asset.Count));

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

                //Учитываем возможное деление на 0
                if(assetReport.Count != 0)
                    assetReport.AvgPrice = (assetReport.Spent - assetReport.SoldOn) / assetReport.Count;

                listAssetsReport.Add(assetReport);
            }

            var coinsData = GetCoursesCoins(listAssetsReport.Select(asset => asset.Name));

            foreach (var asset in listAssetsReport)
            {
                try
                {
                    asset.CurrentPrice = (decimal)coinsData.FirstOrDefault(coin => coin.Symbol == asset.Name).Price;
                }
                catch 
                {
                    asset.CurrentPrice = 0;
                    asset.ProfitPercent = 0;
                    continue;
                }

                asset.AbsoluteProfit = GetAbsoluteProfit(asset.AvgPrice * asset.Count, asset.CurrentPrice * asset.Count);
                asset.ProfitPercent  = GetProcentProfit(asset.AbsoluteProfit, asset.AvgPrice * asset.Count);
            }

            return listAssetsReport;
        }

        private IEnumerable<Currency> GetCoursesCoins(IEnumerable<string> slugList)
        {
            if(string.IsNullOrEmpty(_cmcApiKey))
                return new List<Currency> { new Currency() };

            try
            {
                var client = new CoinmarketcapClient(_cmcApiKey);           //todo скрыть

                return client.GetCurrencyBySymbolList(slugList.ToArray());
            }
            catch
            {
                return new List<Currency> { new Currency() };
            }
        }

        /// <summary>
        /// Абсолютная доходнасть в долларах
        /// </summary>
        /// <param name="avgPrice">Стоимость активов по средней цене</param>
        /// <param name="currentPrice">Стоимость активов по текущей цене</param>
        private static decimal GetAbsoluteProfit(
            decimal avgPrice,
            decimal currentPrice)
        {
            if (currentPrice == 0 || avgPrice == 0)
            {
                return 0;   //значит не найдена цена и нет смысла расчитывать профит
            }

            return currentPrice - avgPrice;
        }

        /// <summary>
        /// Относительная доходность в процентах
        /// </summary>
        /// <param name="absoluteProfit">Абсолютная доходнасть</param>
        /// <param name="avgPrice">Стоимость активов по средней цене</param>
        /// <returns></returns>
        private static decimal GetProcentProfit(
            decimal absoluteProfit,
            decimal avgPrice)
        {
            if (avgPrice == 0)
            {
                return 0;   //значит не найдена цена и нет смысла расчитывать профит
            }

            return absoluteProfit / avgPrice * 100;
        }
    }
}
