using backend.Models;

public class NbpTable
{
    public string table { get; set; }
    public string no { get; set; }
    public string effectiveDate { get; set; }
    public List<NbpRate> rates { get; set; }
}