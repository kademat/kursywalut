using backend.Models;
using backend.Repositories;
using backend.Services;
using Moq;
using System.Net;
using System.Text.Json;

namespace backend.tests.Controllers
{
    public class CurrencyControllerTests()
    {
        [Fact]
        public async Task GetTodayRatesFromRepositoryAsync_ShouldReturnRatesForToday()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var today = DateOnly.FromDateTime(DateTime.Now);
            var testData = new List<NbpTable>
            {
                new NbpTable
                {
                    EffectiveDate = today.ToString(),
                    Rates = new List<NbpRate>
                    {
                        new NbpRate { Currency = "USD", Code = "USD", Mid = 4.2m }
                    }
                },
                new NbpTable
                {
                    EffectiveDate = today.AddDays(-1).ToString(),
                    Rates = new List<NbpRate>
                    {
                        new NbpRate { Currency = "EUR", Code = "EUR", Mid = 4.5m }
                    }
                }
            };

            repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(testData);

            var service = new CurrencyService(repositoryMock.Object, httpClientFactoryMock.Object);

            // Act
            var result = await service.GetTodayRatesFromRepositoryAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result.First().Code);
            repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetRatesFromApiAsync_ShouldSaveRatesToRepository()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            var httpClientMock = new Mock<HttpClient>();

            var ratesFromApi = new List<NbpTable>
            {
                new NbpTable
                {
                    Table = "A",
                    No = "123/A/NBP/2024",
                    EffectiveDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                    Rates = new List<NbpRate>
                    {
                        new NbpRate { Currency = "USD", Code = "USD", Mid = 4.2m }
                    }
                }
            };

            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(ratesFromApi))
            };

            var httpClient = new HttpClient(new MockHttpMessageHandler(fakeResponse));

            httpClientFactoryMock.Setup(f => f.CreateClient("NBP")).Returns(httpClient);

            var service = new CurrencyService(repositoryMock.Object, httpClientFactoryMock.Object);

            // Act
            var result = await service.GetRatesFromApiAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("USD", result.First().Code);
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<NbpTable>()), Times.Once);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_ShouldReturnData_WhenApiRespondsWithValidData()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository>();
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();

            var ratesFromApi = new List<NbpTable>
            {
                new NbpTable
                {
                    Table = "A",
                    No = "123/A/NBP/2024",
                    EffectiveDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                    Rates = new List<NbpRate>
                    {
                        new NbpRate { Currency = "USD", Code = "USD", Mid = 4.2m }
                    }
                }
            };

            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(ratesFromApi))
            };

            var httpClient = new HttpClient(new MockHttpMessageHandler(fakeResponse));

            httpClientFactoryMock.Setup(f => f.CreateClient("NBP")).Returns(httpClient);

            var service = new CurrencyService(repositoryMock.Object, httpClientFactoryMock.Object);

            // Act
            var result = await service.GetCurrencyRatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("A", result.First().Table);
        }
    }
}
