using System.Globalization;
using CsvHelper;
using CsvReadWithApiCheck;
using CsvReadWithApiCheck.Models;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Starting ...");

IEnumerable<CsvRow> csvList;
IConfigurationRoot configurationRoot;
HashSet<string> matchingCompanies = new();
string csvPath = "test-path"

InitConfiguration();
LoadCsvData();

CheckCompanies();

Console.WriteLine($"{matchingCompanies.Count} producers match a lookup SIC code."); //replace

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

void CheckCompanies()
{
    Console.WriteLine("Checking companies against Companies House ...");
    var companiesHouseClient = new CompaniesHouseClient(configurationRoot);

    foreach (var compNumber in csvList.Select(c => c.CompanyNumber))
    {
        var company = companiesHouseClient.GetCompany(compNumber ?? string.Empty);

        if (company?.SicCodes != null)
        {
            bool hasMatch = company.SicCodes.Any(x => CsvReadWithApiCheck.Constants.LookupSicCodes.Any(y => y == x));

            if (hasMatch)
            {
                matchingCompanies.Add(compNumber);
                Console.WriteLine($"{company.Name} is a matching producer.");
            }
        }

        Thread.Sleep(500);
    }
}