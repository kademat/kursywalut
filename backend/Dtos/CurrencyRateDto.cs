namespace backend.Dtos
{
    public class CurrencyRateDto
    {
        public required string Code { get; set; }
        public required string Currency { get; set; }
        public decimal Mid { get; set; }
    }
}
