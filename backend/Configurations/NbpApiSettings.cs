namespace backend.Configurations
{
    public class NbpApiSettings
    {
        public required string BaseUrl { get; set; }
        public required string AverageRatesMainCurrencies { get; set; }
        public required string AverageRatesMinorCurrencies { get; set; }
        public required string AcceptHeader { get; set; }
    }
}
