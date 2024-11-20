using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class NbpRate
    {
        [Key]
        [Required]
        public string Code { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public decimal Mid { get; set; }
    }
}
