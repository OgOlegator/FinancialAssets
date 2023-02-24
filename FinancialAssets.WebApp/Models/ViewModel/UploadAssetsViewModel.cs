﻿namespace FinancialAssets.WebApp.Models.ViewModel
{
    public class UploadAssetsViewModel
    {
        public bool IsLoaded { get; set; } = false;

        public List<Asset> Assets { get; set; }

        public string? Message { get; set; } = "";

    }
}
