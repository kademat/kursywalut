namespace backend.Dtos
{
    public class CurrencyTableDto
    {
        public string No { get; set; }
        public string EffectiveDate { get; set; } // W stringu dla prostoty obsługi w API
        public string Table { get; set; }
        public List<CurrencyRateDto> Rates { get; set; }
    }
}
