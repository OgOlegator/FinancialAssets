﻿namespace FinancialAssets.WebApp.Models
{
    public class ResponseDto
    {

        public bool IsSuccess { get; set; } = true;

        public object Result { get; set; }

        public string DisplayMessage { get; set; } = "";

        public string ErrorMessages { get; set; }

    }
}
