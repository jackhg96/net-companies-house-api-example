using System.Globalization;
using CsvHelper;
using CsvReadWithApiCheck;
using CsvReadWithApiCheck.Models;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting ...");

IEnumerable<CsvRow> csvList;
IConfigurationRoot configurationRoot;
HashSet<string> matchingCompanies = new();
string csvPath = "test-path";
HashSet<string> companiesUsingScheme = new();

InitConfiguration();
LoadCsvData();

await CheckCompanies();

Console.WriteLine($"{matchingCompanies.Count} producers match a lookup SIC code."); 
Console.WriteLine($"Of those producers {companiesUsingScheme.Count} are using a compliance scheme."); 

Console.WriteLine($"Press any key to exit.");

Console.ReadKey();

void InitConfiguration() 
{
    Console.WriteLine("Loading ...");

    var builder = new ConfigurationBuilder()
            .AddUserSecrets<Program>();

    configurationRoot = builder.Build();
}

void LoadCsvData()
{
    Console.WriteLine("Retrieving CSV data ...");

    using (var reader = new StreamReader(csvPath))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    
    csvList = csv.GetRecords<CsvRow>().ToList();

    Console.WriteLine($"{csvList.Count()} records found in csv.");
}

async Task CheckCompanies()
{
    Console.WriteLine("Checking companies against Companies House ...");

    var companiesHouseClient = new CompaniesHouseClient(configurationRoot);

    foreach (var row in csvList)
    {
        var company = await companiesHouseClient.GetCompanyAsync(row.CompanyNumber ?? string.Empty);

        if (company?.SicCodes != null)
        {
            bool hasMatch = company.SicCodes.Any(x => CsvReadWithApiCheck.Constants.LookupSicCodes.Any(y => y == x));

            if (hasMatch)
            {
                matchingCompanies.Add(row.CompanyNumber);

                var sanitisedString = row.Scheme.Replace("\r", string.Empty);

                Console.WriteLine($"{company.Name} is a matching producer using the scheme {sanitisedString}.");

                if (!sanitisedString.Contains("DirectRegistrant"))
                {
                    companiesUsingScheme.Add(row.CompanyNumber);
                }
            }
            else
            {
                Console.WriteLine($"{company.Name} is NOT a matching producer.");
            }
        }

        Thread.Sleep(500);
    }
}