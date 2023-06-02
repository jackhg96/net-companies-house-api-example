using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CsvReadWithApiCheck.Models;
using Microsoft.Extensions.Configuration;
using Polly;

namespace CsvReadWithApiCheck;

public class CompaniesHouseClient
{
    private readonly HttpClient httpClient;
    private readonly string apiKey;

    public CompaniesHouseClient(IConfigurationRoot config)
    {
        apiKey = config.GetSection(Constants.ApiKeySection).Value;
        httpClient = new HttpClient
        {
            BaseAddress = new Uri(Constants.CompaniesHouseBaseUri)
        };
    }

    public async Task<GetCompanyResponse?> GetCompanyAsync(string companyNumber)
    {
        try
        {
            var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);

            var paddedString = companyNumber.PadLeft(8, '0');

            var result = await Policy
              .Handle<HttpRequestException>()
              .OrResult<HttpResponseMessage>(r => Constants.HttpStatusCodesWorthRetrying.Contains(r.StatusCode))
              .WaitAndRetryAsync(new[]
              {
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
              },
              (a, b, c) => Console.WriteLine($"Error occurred with {paddedString}, retrying..."))
              .ExecuteAsync(() => httpClient.GetAsync($"company/{paddedString}"));

            var jsonString = await result.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<GetCompanyResponse>(jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Company number {companyNumber}: {ex.Message}");
            return null;
        }
    }
}