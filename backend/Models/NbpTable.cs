using backend.Models;

public class NbpTable
{
    public string No { get; set; }
    public string EffectiveDate { get; set; }
    public string Table { get; set; }
    public List<NbpRate> Rates { get; set; }
}