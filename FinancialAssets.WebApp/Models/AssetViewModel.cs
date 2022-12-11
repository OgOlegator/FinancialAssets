namespace FinancialAssets.WebApp.Models
{
    public class AssetViewModel
    {

        public IEnumerable<Asset> Assets { get; set; }

        public string? SearchCoin { get; set; }

    }
}
