using backend.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class NbpTable
{
    [Key]
    public required string No { get; set; }
    /// <summary>
    /// DateOnly jest nowszym typem dodanym w .NET6, kt�ry idealnie pasuje do formatu z dokumentacji jakim jest
    /// data w formacie RRRR-MM-DD (standard ISO 8601)
    /// </summary>
    public required DateOnly EffectiveDate { get; set; }
    public required string Table { get; set; }
    public required List<NbpRate> Rates { get; set; } // Relacja jeden-do-wielu z NbpRate
}