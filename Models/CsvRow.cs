using CsvHelper.Configuration.Attributes;

namespace CsvReadWithApiCheck.Models;

public class CsvRow
{
    [Name("Company")] 
    public string? CompanyNumber { get; set; }

    [Name("Producer Organisation")]
    public string? CompanyName { get; set; }
}
