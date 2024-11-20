using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class NbpRate
    {
        public string currency { get; set; }
        [Key]
        public string code { get; set; }
        public decimal mid { get; set; }
    }
}
