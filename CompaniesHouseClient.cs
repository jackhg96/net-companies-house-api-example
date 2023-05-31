using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CsvReadWithApiCheck.Models;
using Microsoft.Extensions.Configuration;

namespace CsvReadWithApiCheck;

public class CompaniesHouseClient
{
    private const string CompaniesHouseBaseUri = "https://api.company-information.service.gov.uk/";
    private const string ApiKeySection = "CompaniesHouseApiKey";
    private readonly HttpClient httpClient;
    private readonly string apiKey;

    public CompaniesHouseClient(IConfigurationRoot config)
    {
        apiKey = config.GetSection(ApiKeySection).Value;
        httpClient = new HttpClient
        {
            BaseAddress = new Uri(CompaniesHouseBaseUri)
        };
    }

    public GetCompanyResponse? GetCompany(string companyNumber) 
    {
        var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);

        var paddedString = companyNumber.PadLeft(8, '0');

        var res = httpClient.GetAsync($"company/{paddedString}").Result;
        var json = res.Content.ReadAsStringAsync().Result;

        return JsonSerializer.Deserialize<GetCompanyResponse>(json);
    }
}