namespace FinancialAssets.WebApp.Models.ViewModel
{
    public class WalletAssetIndexViewModel
    {

        public IEnumerable<WalletAsset> WalletAssets { get; set; }

        public string? SearchCoin { get; set; }

        public string? SearchWallet { get; set; }

    }
}
