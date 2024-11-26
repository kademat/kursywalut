using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class NbpRate
    {
        [Key]
        public required string Code { get; set; }
        public required string Currency { get; set; }
        public decimal Mid { get; set; }
        public NbpTable? NbpTable { get; set; }  // Relacja do tabeli NbpTable
    }
}
