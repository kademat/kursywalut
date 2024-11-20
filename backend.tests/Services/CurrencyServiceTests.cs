using backend.Models;
using backend.Repositories;
using backend.Services;
using Moq;
using Moq.Protected;
using System.Net;

public class CurrencyServiceTests
{
    /// <summary>
    /// Repozytorium zawiera dane z kursami walut z dnia dzisiejszego
    /// </summary>
    /// <returns>Repozytorium zwraca kursy z dnia dzisiejszego</returns>
    [Fact]
    public async Task GetTodayRatesFromRepository_ReturnsRatesForToday()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now);
        var rates = new List<NbpRate>
        {
            new() { Currency = "USD", Code = "USD", Mid = 4.2m },
            new() { Currency = "EUR", Code = "EUR", Mid = 4.5m }
        };
        var expectedResult = new List<NbpTable>
        {
            new() { No = "No", Table = "A", EffectiveDate = today, Rates = rates }
        };

        var repositoryMock = new Mock<IRepository>();
        repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedResult);

        var httpClientMock = new Mock<HttpClient>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClientMock.Object);

        var _currencyService = new CurrencyService(repositoryMock.Object, httpClientFactoryMock.Object);

        // Act
        var result = await _currencyService.GetCurrencyRatesAsync();

        // Assert

        Assert.Collection(result,
            item =>
            {
                Assert.Equal(expectedResult[0].Rates[0].Currency, item.Rates[0].Currency);
                Assert.Equal(expectedResult[0].Rates[0].Code, item.Rates[0].Code);
                Assert.Equal(expectedResult[0].Rates[0].Mid, item.Rates[0].Mid);
            });
        // DYSKUSJA: czy to naruszenie SRP? Scenariusz a powód zmiany
        repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    /// <summary>
    /// Repozytorium nie zawiera danych
    /// </summary>
    /// <returns>Zwracana jest pusta lista z repozytorium</returns>
    [Fact]
    public async Task GetRates_EmptyRepository_ReturnsDataFromAPI()
    {
        // Arrange
        var expectedContent = "[{ \"Table\": \"A\", \"No\": \"123\", \"EffectiveDate\": \"2024-11-20\", \"Rates\": [{ \"Currency\": \"USD\", \"Code\": \"USD\", \"Mid\": 4.2 }] }]";
        var _repositoryMock = new Mock<IRepository>();
        // repozytorium jest puste
        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()  // używamy metody Protected aby zamokować użycie chronionych elementów HttpMessageHandler
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedContent),
            });


        var httpClientMock = new Mock<HttpClient>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClientMock.Object);


        var currencyService = new CurrencyService(_repositoryMock.Object, httpClientFactoryMock.Object);

        // Act
        var result = await currencyService.GetCurrencyRatesAsync();

        // Assert
        Assert.NotEmpty(result);
    }
}