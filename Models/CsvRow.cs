using CsvHelper.Configuration.Attributes;

namespace CsvReadWithApiCheck.Models;

public class CsvRow
{
    [Name("Company")]
    public string CompanyNumber { get; set; } = string.Empty;

    [Name("Producer Organisation")]
    public string CompanyName { get; set; } = string.Empty;
    
    [Name("Scheme")]
    public string Scheme { get; set; } = string.Empty;
}
