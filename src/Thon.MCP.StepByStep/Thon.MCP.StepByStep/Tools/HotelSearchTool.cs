using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;

namespace Thon.CustomerMcp.Functions;

public class HotelSearchTool
{
    [Function("SearchHotel")]
    public async Task<IActionResult> Run(
        [McpToolTrigger("search_hotel", "Search for Thon Hotels using various search criteria such as location, name, or facilities. Returns a list of matching hotels with their details.")] ToolInvocationContext context,
        [McpToolProperty("query", "string", "Mandatory! Search query string to find Thon Hotels by name, location, facilities, or other criteria.", true)] string query)
    {
        var allHotels = GetAllHotels();
        var filteredHotels = SearchHotels(allHotels, query);

        return new OkObjectResult(filteredHotels);
    }

    private static object[] GetAllHotels()
    {
        return new[]
        {
            new
            {
                Navn = "Thon Hotel Oslo Airport",
                Adresse = "Hans Gaarders veg 15, 2060 Gardermoen",
                Telefonnummer = "+47 64 84 00 00",
                Organisasjonsnummer = "123456789",
                Antallrom = 151,
                Fasiliteter = new[] { "Restaurant", "Bar", "Treningssenter", "Gratis WiFi" },
                Status = "Aktiv"
            },
            new
            {
                Navn = "Thon Hotel Bristol Oslo",
                Adresse = "Kristian IVs gate 7, 0164 Oslo",
                Telefonnummer = "+47 22 82 60 00",
                Organisasjonsnummer = "234567890",
                Antallrom = 252,
                Fasiliteter = new[] { "Restaurant", "Bar", "Konferanserom", "Gratis WiFi" },
                Status = "Aktiv"
            },
            new
            {
                Navn = "Thon Hotel Rosenkrantz Oslo",
                Adresse = "Rosenkrantz' gate 1, 0159 Oslo",
                Telefonnummer = "+47 23 31 55 00",
                Organisasjonsnummer = "345678901",
                Antallrom = 151,
                Fasiliteter = new[] { "Restaurant", "Bar", "Konferanserom", "Gratis WiFi", "Parkering" },
                Status = "Aktiv"
            },
            new
            {
                Navn = "Thon Hotel Vika Atrium Oslo",
                Adresse = "Munkedamsveien 45, 0250 Oslo",
                Telefonnummer = "+47 23 31 53 00",
                Organisasjonsnummer = "456789012",
                Antallrom = 93,
                Fasiliteter = new[] { "Restaurant", "Konferanserom", "Gratis WiFi" },
                Status = "Aktiv"
            },
            new
            {
                Navn = "Thon Hotel Bergen Airport",
                Adresse = "Flyplassveien 555, 5869 Bergen",
                Telefonnummer = "+47 56 17 42 00",
                Organisasjonsnummer = "567890123",
                Antallrom = 210,
                Fasiliteter = new[] { "Restaurant", "Bar", "Treningssenter", "Gratis WiFi" },
                Status = "Aktiv"
            }
        };
    }

    private static object[] SearchHotels(object[] hotels, string query)
    {
        var searchQuery = query?.ToLowerInvariant() ?? string.Empty;

        return hotels.Where(hotel =>
        {
            var hotelType = hotel.GetType();
            var navn = hotelType.GetProperty("Navn")?.GetValue(hotel)?.ToString() ?? string.Empty;
            var adresse = hotelType.GetProperty("Adresse")?.GetValue(hotel)?.ToString() ?? string.Empty;
            var fasiliteter = hotelType.GetProperty("Fasiliteter")?.GetValue(hotel) as string[] ?? Array.Empty<string>();

            return navn.ToLowerInvariant().Contains(searchQuery) ||
                   adresse.ToLowerInvariant().Contains(searchQuery) ||
                   fasiliteter.Any(f => f.ToLowerInvariant().Contains(searchQuery));
        }).ToArray();
    }
}
