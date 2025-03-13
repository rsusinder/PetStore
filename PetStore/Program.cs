using System.Text.Json;
using System.Text.Json.Serialization;
using ApiSdk;
using ApiSdk.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using PetStore;

JsonSerializerOptions jsonSerializerOptions = new()
{
    WriteIndented = true
};
jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

HttpClient httpClient = new(new SanitizingHttpHandler(new HttpClientHandler()));



ApiKeyAuthenticationProvider authenticationProvider = new PetStoreAuthenticationProvider("special-key");
HttpClientRequestAdapter requestAdapter = new(authenticationProvider, httpClient: httpClient);

ApiClient client = new(requestAdapter);

List<ApiSdk.Models.Pet> availablePets = await Helper.FindAvailablePets(client, jsonSerializerOptions);


// Print grouped and sorted pets
foreach (KeyValuePair<string, List<Pet>> category in Helper.GroupAndSortPets(availablePets))
{
    Console.WriteLine($"Category: {category.Key}");
    foreach (Pet? pet in category.Value)
    {
        Console.WriteLine($"  ID: {pet.Id}, Name: {pet.Name}");
    }
    Console.WriteLine();
}



