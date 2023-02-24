namespace FinancialAssets.WebApp.Models.ViewModel
{
    public class WalletAssetAddViewModel
    {

        public WalletAsset WalletAsset { get; set; }

        public string? Message { get; set; }

        public bool ShowMessage => !string.IsNullOrWhiteSpace(Message);

    }
}
