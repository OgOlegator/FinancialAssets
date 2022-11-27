﻿using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Services.IServices;
using NoobsMuc.Coinmarketcap.Client;
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

            var coinsData = GetCoursesCoins(listAssetsReport.Select(asset => asset.Name));

            foreach (var asset in listAssetsReport)
            {
                try
                {
                    asset.CurrentPrice = (decimal)coinsData.FirstOrDefault(coin => coin.Symbol == asset.Name).Price;
                    asset.ProfitPercent = (asset.AvgPrice / asset.CurrentPrice) * 100 - 100;
                    asset.ProfitPercent = asset.AvgPrice > asset.CurrentPrice ? -asset.ProfitPercent : asset.ProfitPercent;
                }
                catch
                {
                    asset.CurrentPrice = 0;
                    asset.ProfitPercent = 0;
                    continue;
                }
            }

            return listAssetsReport;
        }

        private IEnumerable<Currency> GetCoursesCoins(IEnumerable<string> slugList)
        {
            var client = new CoinmarketcapClient("f4c1a066-3bab-4d95-b1a6-e766eb4ddaaa");           //todo скрыть

            return client.GetCurrencyBySymbolList(slugList.ToArray());
        }
    }
}
