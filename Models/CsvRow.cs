using CsvHelper.Configuration.Attributes;

namespace CsvReadWithApiCheck.Models;

public class CsvRow
{
    [Name("company_number")] 
    public string? ComanyNumber { get; set; }

    [Name("company_name")]
    public string? CompanyName { get; set; }
}
