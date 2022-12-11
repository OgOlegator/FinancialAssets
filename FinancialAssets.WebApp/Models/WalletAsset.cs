using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssets.WebApp.Models
{
    [PrimaryKey(nameof(Coin), nameof(Wallet))]
    public class WalletAsset
    {
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Coin { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Wallet { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,12)")]
        public decimal Count { get; set; }

    }
}
