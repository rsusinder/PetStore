using Microsoft.Kiota.Abstractions.Authentication;
namespace PetStore;

internal sealed class PetStoreAuthenticationProvider : ApiKeyAuthenticationProvider
{
    public PetStoreAuthenticationProvider(string apiKey) : base("Bearer " + apiKey, "Authorization", KeyLocation.Header)
    {
    }
}
