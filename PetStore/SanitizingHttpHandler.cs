using System.Text;
using System.Text.RegularExpressions;
namespace PetStore;

internal class SanitizingHttpHandler : DelegatingHandler
{
    public SanitizingHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        if (response.Content != null)
        {
            string json = await response.Content.ReadAsStringAsync();

            // Remove invalid control characters
            json = Regex.Replace(json, @"[\x00-\x08\x0B\x0C\x0E-\x1F]", "");

            // Replace content with sanitized JSON
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return response;
    }
}
