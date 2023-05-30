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

Console.WriteLine($"{csvList.Count()} records found in csv.");

Console.WriteLine($"{matchingCompanies.Count} producers match a Cups SIC code.");

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
}

void CheckCompanies()
{
    Console.WriteLine("Checking companies against Companies House ...");
    var companiesHouseClient = new CompaniesHouseClient(configurationRoot);

    foreach (var row in csvList)
    {
        var company = companiesHouseClient.GetCompany(row.ComanyNumber);
        bool hasMatch = company.SicCodes.Any(x => CsvReadWithApiCheck.Constants.CupsSicCodes.Any(y => y == x));
        
        if (hasMatch)
        {
            matchingCompanies.Add(row.ComanyNumber);
        }
    }
}