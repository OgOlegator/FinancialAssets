namespace FinancialAssets.WebApp.Models.ViewModel
{
    public class SaveViewModel
    {

        public List<Asset> ErrorAssets { get; set; }

        public string? Message { get; set; } = "";

        public bool IsSave { get; set; } = false;

    }
}
