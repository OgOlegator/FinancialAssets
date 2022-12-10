﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinancialAssets.WebApp.Models
{
    public class WalletAsset
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,12)")]
        public decimal Count { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Wallet { get; set; }
    }
}
