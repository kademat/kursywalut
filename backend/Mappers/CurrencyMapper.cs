using backend.Dtos;
using backend.Models;

namespace backend.Mappers
{
    public static class CurrencyMapper
    {
        public static CurrencyRateDto ToDto(this NbpRate rate)
        {
            return new CurrencyRateDto
            {
                Code = rate.Code,
                Currency = rate.Currency,
                Mid = rate.Mid
            };
        }

        public static NbpRate ToDomain(this CurrencyRateDto dto)
        {
            return new NbpRate
            {
                Code = dto.Code,
                Currency = dto.Currency,
                Mid = dto.Mid
            };
        }

        public static CurrencyTableDto ToDto(this NbpTable table)
        {
            return new CurrencyTableDto
            {
                No = table.No,
                EffectiveDate = table.EffectiveDate.ToString("yyyy-MM-dd"),
                Table = table.Table,
                Rates = table.Rates.Select(r => r.ToDto()).ToList()
            };
        }

        public static NbpTable ToDomain(this CurrencyTableDto dto)
        {
            return new NbpTable
            {
                No = dto.No,
                EffectiveDate = DateOnly.Parse(dto.EffectiveDate),
                Table = dto.Table,
                Rates = dto.Rates.Select(r => r.ToDomain()).ToList()
            };
        }
    }
}