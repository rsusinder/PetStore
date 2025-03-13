using System.Runtime.CompilerServices;
using System.Text.Json;
using ApiSdk;
using ApiSdk.Models;

[assembly: InternalsVisibleTo("PetStore.Tests")]
namespace PetStore;

internal class Helper
{
    public static async Task<List<ApiSdk.Models.Pet>> FindAvailablePets(ApiClient client, JsonSerializerOptions jsonSerializerOptions)
    {
        string[] statuses = new[] { "available" }; // Mutliple statuses we are going to query on.

        List<ApiSdk.Models.Pet>? results = (await client.Pet.FindByStatus.GetAsync(x => x.QueryParameters.Status = statuses))?.ToList();
        if (results is null)
        {
            Console.WriteLine("No results found.");
            return Enumerable.Empty<ApiSdk.Models.Pet>().ToList();
        }
        return results;
    }

    public static Dictionary<string, List<Pet>> GroupAndSortPets(List<ApiSdk.Models.Pet> availablePets)
    {
        // Group pets by category name and sort in reverse order by name
        return availablePets
            .Where(p => p.Name != null) // Ensure name is not null
            .GroupBy(p => p.Category?.Name ?? "Uncategorized")
            .OrderBy(g => g.Key)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(p => p.Name).ToList()
            );
    }
}
