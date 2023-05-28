using System.ComponentModel.DataAnnotations;

namespace FinancialAssets.WebApp.Models
{
    public class FullReport
    {
        public List<AssetReport> Assets { get; set; } = new List<AssetReport>();

        [Display(Name = "Потрачено средств")]
        public decimal TotalSpent { get; set; }

        [Display(Name = "Продаж на сумму")]
        public decimal TotalSoldOn { get; set; }

        [Display(Name = "Прибыль в процентах")]
        public decimal ProfitPercent { get; set; }  //Разница цен покупки и текущей по всему портфелю в процентах

        [Display(Name = "Прибыль в долларах")]
        public decimal AbsoluteProfit { get; set; }  //Разница цен покупки и текущей по всему портфелю в долларах
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

        [Display(Name = "Прибыль в процентах")]
        public decimal ProfitPercent { get; set; }

        [Display(Name = "Прибыль в долларах")]
        public decimal AbsoluteProfit { get; set; }

        [Display(Name = "Потрачено средств")]
        public decimal Spent { get; set; }

        [Display(Name = "Продаж на сумму")]
        public decimal SoldOn { get; set; }

    }
}
