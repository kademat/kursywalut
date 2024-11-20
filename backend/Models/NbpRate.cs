using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class NbpRate
    {
        [Key]
        public required string Code { get; set; }
        public required string Currency { get; set; }
        public decimal Mid { get; set; }
    }
}
