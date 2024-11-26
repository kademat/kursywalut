namespace backend.Dtos
{
    public class CurrencyTableDto
    {
        public required string No { get; set; }
        public required string EffectiveDate { get; set; } // W stringu dla prostoty obsługi w API
        public required string Table { get; set; }
        public List<CurrencyRateDto> Rates { get; set; }
    }
}
