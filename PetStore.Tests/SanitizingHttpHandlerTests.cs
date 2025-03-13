using System.Net;
using System.Text;

namespace PetStore.Tests
{
    public class SanitizingHttpHandlerTests
    {
        [Fact]
        public async Task SendAsync_RemovesInvalidCharacters_FromJsonResponse()
        {
            // Arrange: Create a fake JSON response with invalid characters
            string invalidJson = "{ \"name\": \"Pet\x19Store\", \"status\": \"available\" }";
            string expectedJson = "{ \"name\": \"PetStore\", \"status\": \"available\" }"; // Expected output after sanitization

            HttpResponseMessage fakeResponse = new(HttpStatusCode.OK)
            {
                Content = new StringContent(invalidJson, Encoding.UTF8, "application/json")
            };

            MockHttpMessageHandler mockHandler = new(fakeResponse);
            SanitizingHttpHandler sanitizingHandler = new(mockHandler);
            HttpClient httpClient = new(sanitizingHandler);

            // Act: Send a request through the handler
            HttpResponseMessage response = await httpClient.GetAsync("https://fakeapi.com/test");
            string sanitizedContent = await response.Content.ReadAsStringAsync();

            // Assert: Ensure invalid characters are removed
            Assert.Equal(expectedJson, sanitizedContent);
        }
    }

    // Mock HttpMessageHandler to simulate HTTP responses
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public MockHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}
