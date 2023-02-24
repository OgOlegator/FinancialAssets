using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Models.Dtos;
using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Services.IServices;
using System.IO;

namespace FinancialAssets.WebApp.Services
{
    public class CsvUploader : ICsvUploader
    {
        private readonly IAssetRepository _repository;
        
        public CsvUploader(IAssetRepository repository)
        {
            _repository = repository; 
        }

        public async Task<ResponseDto> Parse(IFormFile uploadedFile)
        {
            byte[] fileData;

            using (var fileStream = uploadedFile.OpenReadStream())
            {
                try
                {
                    fileData = new byte[fileStream.Length];
                    await fileStream.ReadAsync(fileData, 0, (int)fileStream.Length);
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
            }

            var dataLines = System.Text.Encoding.ASCII.GetString(fileData).Split("\r\n");

            var assetList = new List<Asset>();

            for (var i = 1; i < dataLines.Length; i++)
            {
                var lineItems = dataLines[i].Split(";");

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

            return new ResponseDto
            {
                IsSuccess = true,
                Result = assetList
            };
        }

        public async Task<ResponseDto> Upload(object data)
        {
            try
            {
                foreach (var asset in (List<Asset>)data)
                {
                    await _repository.AddAsset(asset);
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
