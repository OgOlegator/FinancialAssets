using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Services.IServices;
using System.IO;

namespace FinancialAssets.WebApp.Services
{
    public class CsvUploader : IUploader
    {
        private readonly IAssetRepository _repository;
        
        public CsvUploader(IAssetRepository repository)
        {
            _repository = repository; 
        }

        public async Task<bool> UploadAsync(IFormFile uploadedFile)
        {
            var assetList = new List<Asset>();

            byte[] fileData;

            using (var fileStream = uploadedFile.OpenReadStream())
            {
                fileData = new byte[fileStream.Length];
                await fileStream.ReadAsync(fileData, 0, (int)fileStream.Length);
            }

            var dataLines = System.Text.Encoding.ASCII.GetString(fileData).Split("\r\n");

            if (dataLines.Length == 0)
            {
                return true;
            }

            for(var i = 1; i < dataLines.Length; i++)
            {
                var lineItems = dataLines[i].Split(";");

                var date = string.IsNullOrEmpty(lineItems[4]) ? DateTime.Now : DateTime.Parse(lineItems[4]);
                var count = decimal.Parse(lineItems[2]);

                assetList.Add(new Asset
                {
                    Name = lineItems[0],
                    Price = decimal.Parse(lineItems[1]),
                    Count = decimal.Parse(lineItems[2]),
                    Operation = lineItems[3],
                    Date = string.IsNullOrEmpty(lineItems[4]) ? DateTime.Now : DateTime.Parse(lineItems[4]),
                    Marketplace = lineItems[5]
                });
            }

            foreach (var asset in assetList)
                await _repository.AddAsset(asset);

            return true;
        }
    }
}
