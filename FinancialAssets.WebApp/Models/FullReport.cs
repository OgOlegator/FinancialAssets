using System.ComponentModel.DataAnnotations;

namespace FinancialAssets.WebApp.Models
{
    public class FullReport
    {

        public List<AssetReport> Assets { get; set; }

        [Display(Name = "Потрачено средств")]
        public decimal TotalSpent { get; set; }

        [Display(Name = "Продаж на сумму")]
        public decimal SoldOn { get; set; }

        [Display(Name = "Profit")]
        public decimal ProfitPercent { get; set; }  //Разница цен покупки и текущей по всему портфелю
    }

    public class AssetReport
    {
        [Display(Name = "Актив")]
        public string Name { get; set; }

        [Display(Name = "Количество")]
        public decimal Count { get; set; }

        [Display(Name = "Средняя цена")]
        public decimal AvgPrice { get; set; }

        [Display(Name = "Текущая цена")]
        public decimal CurrentPrice { get; set; }

        [Display(Name = "Profit")]
        public decimal ProfitPercent { get; set; }

        [Display(Name = "Потрачено средств")]
        public decimal TotalSpent { get; set; }

    }
}
