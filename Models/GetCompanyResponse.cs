using System.Text.Json.Serialization;

namespace CsvReadWithApiCheck.Models;

public class GetCompanyResponse
{
    [JsonPropertyName("sic_codes")]
    public string[]? SicCodes { get; set; }

    [JsonPropertyName("company_number")]
    public string? Number { get; set; }

    [JsonPropertyName("company_name")]
    public string? Name { get; set; }
}
