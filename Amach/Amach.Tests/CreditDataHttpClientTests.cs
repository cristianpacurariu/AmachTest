using Amach.HttpClients.CreditData;
using Amach.HttpClients.CreditData.Dtos;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;

namespace Amach.Tests
{
    public class CreditDataHttpClientTests
    {
        private readonly Mock<HttpMessageHandler> _handler = new();
        private readonly CreditDataHttpClient _sut;

        public CreditDataHttpClientTests()
        {
            var client = _handler.CreateClient();
            client.BaseAddress = new Uri(CreditDataHttpClient.Url);
            _sut = new CreditDataHttpClient(client);
        }

        [Theory]
        [InlineData("424-11-9327")]
        //[InlineData("553-25-8346")]
        //[InlineData("287-54-7823")]
        public async Task GetPersonalDetailsAsync_ReturnsOk(string ssn)
        {
            var dto = new PersonalDetailsDto
            {
                Address = "address",
                FirstName = "first",
                LastName = "last"
            };

            _handler
                .SetupRequest($"{CreditDataHttpClient.Url}/credit-data/{ssn}", requestMessage =>
                {
                    requestMessage.Method = HttpMethod.Get;
                    return true;
                })
                .ReturnsResponse(HttpStatusCode.OK, responseMessage =>
                {
                    responseMessage.Content = new StringContent(JsonSerializer.Serialize(dto));
                });

            var result = await _sut.GetPersonalDetailsAsync(ssn, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}