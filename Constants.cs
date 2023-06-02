using System.Collections.Immutable;
using System.Net;

namespace CsvReadWithApiCheck;

public static class Constants
{
    public const string CompaniesHouseBaseUri = "https://api.company-information.service.gov.uk/";

    public const string ApiKeySection = "CompaniesHouseApiKey";

    public static readonly ImmutableHashSet<string> LookupSicCodes = ImmutableHashSet.Create(
        "47810",
        "93210",
        "47990",
        "55900",
        "93199",
        "55201",
        "55202",
        "55209",
        "47250",
        "93290",
        "55300",
        "93130",
        "47290",
        "47240",
        "93110",
        "93120",
        "47760",
        "56210",
        "47190",
        "55100",
        "56290",
        "47110",
        "56301",
        "56302",
        "56101",
        "56102",
        "49100",
        "49311",
        "52212",
        "52213",
        "49319",
        "49390",
        "50100",
        "50300",
        "51101",
        "51102",
        "50200",
        "59140",
        "59131",
        "10832" //10832 - Production of coffee and coffee substitutes
    );

    public readonly static HttpStatusCode[] HttpStatusCodesWorthRetrying = new HttpStatusCode[]
    {
        HttpStatusCode.BadRequest, // 400
        HttpStatusCode.RequestTimeout, // 408
        HttpStatusCode.Conflict, // 409
        HttpStatusCode.InternalServerError, // 500
        HttpStatusCode.BadGateway, // 502
        HttpStatusCode.ServiceUnavailable, // 503
        HttpStatusCode.GatewayTimeout // 504
    };
}