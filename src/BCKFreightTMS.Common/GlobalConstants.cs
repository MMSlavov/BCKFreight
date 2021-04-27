namespace BCKFreightTMS.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "BCKFreightTMS";

        public const string AdministratorRoleName = "Administrator";

        public const string RegistryAgencyUrlSearch = "https://papagal.bg/search_results/{0}?type=company";

        public const string RegistryAgencyUrl = "https://papagal.bg{0}";

        public const string SystemEmail = "mmslavov00@gmail.com";

        public const string Index = "Index";

        public const string JsonDataPath = @"wwwroot\data\{0}";

        public const string PdfMimeType = "application/pdf";

        public const string ExchangeRateUrl = @"http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";

        public const decimal MinOrderMargin = 0.05m;

        public const decimal SmallOrderMaxAmount = 500;

        public const decimal SmallOrderMinMargin = 25;

        public const decimal VAT = 0.2m;

        public const decimal VATTot = 1.2m;

        public const string BGOrderNumberFormat = "{0}{1}{2}";

        public const string NonBGOrderNumberFormat = "{1}-{0}{2}";

        public const string InvoiceOutNumberFormat = "07{0}";

        public static readonly int[] DueDaysValues = { 15, 30, 45 };
    }
}
