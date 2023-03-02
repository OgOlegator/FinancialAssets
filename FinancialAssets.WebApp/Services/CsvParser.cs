using FinancialAssets.WebApp.Models;
using FinancialAssets.WebApp.Models.Dtos;
using FinancialAssets.WebApp.Repository;
using FinancialAssets.WebApp.Services.IServices;
using Microsoft.AspNetCore.Connections;
using System.IO;

namespace FinancialAssets.WebApp.Services
{
    public class CsvParser : IParser
    {
        private readonly IAssetRepository _repository;

        private readonly List<string> _fileTypes = new List<string> { "csv",  };
        
        public CsvParser(IAssetRepository repository)
        {
            _repository = repository; 
        }

        public async Task<ResponseDto> Parse(IFormFile uploadedFile)
        {
            if(!_fileTypes.Contains(uploadedFile.ContentType.Split("//").Last()))
                return new ResponseDto
                {
                    IsSuccess = false,
                    DisplayMessage = "Неподдерживаемый тип файла"
                };

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
                        DisplayMessage = "Некорректный файл"
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

    }
}
