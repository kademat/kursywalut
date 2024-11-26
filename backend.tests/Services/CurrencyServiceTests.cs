using backend.Configurations;
using backend.Models;
using backend.Repositories;
using backend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;

public class CurrencyServiceTests
{
    /// <summary>
    /// Repozytorium zawiera dane z głównymi kursami walut z dnia dzisiejszego (tabela A)
    /// </summary>
    /// <returns>Repozytorium zwraca kursy z dnia dzisiejszego z tabeli A</returns>
    [Fact]
    public async Task GetTodayMainRatesFromRepository_ReturnsMainRatesForToday()
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

        var fakeOptions = Options.Create(new NbpApiSettings
        {
            BaseUrl = "https://fake.api/",
            AverageRatesMainCurrencies = "test/endpoint",
            AverageRatesMinorCurrencies = "test/minor/endpoint",
            AcceptHeader = "text/json"
        });
        var loggerMock = new Mock<ILogger<CurrencyService>>();

        var currencyService = new CurrencyService(repositoryMock.Object, httpClientFactoryMock.Object, fakeOptions, loggerMock.Object);

        // Act
        var result = await currencyService.GetCurrencyRatesAsync(CurrencyTable.MainCurrencyTable);

        // Assert

        Assert.Collection(result,
            item =>
            {
                Assert.Equal(expectedResult[0].Rates[0].Currency, item.Currency);
                Assert.Equal(expectedResult[0].Rates[0].Code, item.Code);
                Assert.Equal(expectedResult[0].Rates[0].Mid, item.Mid);
            },
            item =>
            {
                Assert.Equal(expectedResult[0].Rates[1].Currency, item.Currency);
                Assert.Equal(expectedResult[0].Rates[1].Code, item.Code);
                Assert.Equal(expectedResult[0].Rates[1].Mid, item.Mid);
            });
        // DYSKUSJA: czy to naruszenie SRP? Scenariusz a powód zmiany
        repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    /// <summary>
    /// Repozytorium zawiera dane z rzadkimi kursami walut z dnia dzisiejszego (tabela B)
    /// </summary>
    /// <returns>Repozytorium zwraca kursy z dnia dzisiejszego z tabeli B</returns>
    [Fact]
    public async Task GetTodayMinorRatesFromRepository_ReturnsMinorRatesForToday()
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
            new() { No = "No", Table = "B", EffectiveDate = today, Rates = rates }
        };

        var repositoryMock = new Mock<IRepository>();
        repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedResult);

        var httpClientMock = new Mock<HttpClient>();
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClientMock.Object);

        var fakeOptions = Options.Create(new NbpApiSettings
        {
            BaseUrl = "https://fake.api/",
            AverageRatesMainCurrencies = "test/endpoint",
            AverageRatesMinorCurrencies = "test/minor/endpoint",
            AcceptHeader = "text/json"
        });
        var loggerMock = new Mock<ILogger<CurrencyService>>();

        var currencyService = new CurrencyService(repositoryMock.Object, httpClientFactoryMock.Object, fakeOptions, loggerMock.Object);

        // Act
        var result = await currencyService.GetCurrencyRatesAsync(CurrencyTable.MinorCurrencyTable);

        // Assert

        Assert.Collection(result,
            item =>
            {
                Assert.Equal(expectedResult[0].Rates[0].Currency, item.Currency);
                Assert.Equal(expectedResult[0].Rates[0].Code, item.Code);
                Assert.Equal(expectedResult[0].Rates[0].Mid, item.Mid);
            },
            item =>
            {
                Assert.Equal(expectedResult[0].Rates[1].Currency, item.Currency);
                Assert.Equal(expectedResult[0].Rates[1].Code, item.Code);
                Assert.Equal(expectedResult[0].Rates[1].Mid, item.Mid);
            });
        // DYSKUSJA: czy to naruszenie SRP? Scenariusz a powód zmiany
        repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    /// <summary>
    /// Repozytorium nie zawiera danych
    /// </summary>
    /// <returns>Zwracana jest lista z API</returns>
    [Fact]
    public async Task GetRates_EmptyRepository_ReturnsDataFromAPI()
    {
        // Arrange
        var _repositoryMock = new Mock<IRepository>();
        // repozytorium jest puste
        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

        var handlerMock = new Mock<HttpMessageHandler>();
        var today = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd");
        var expectedContent = "[{ \"Table\": \"A\", \"No\": \"123\", \"EffectiveDate\": \"" + today + "\", \"Rates\": [{ \"Currency\": \"USD\", \"Code\": \"USD\", \"Mid\": 4.2 }] }]";

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

        var httpClientMock = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://fake.api/") // Ustawienie BaseAddress bezpośrednio w HttpClient
        };

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClientMock);

        var fakeOptions = Options.Create(new NbpApiSettings
        {
            BaseUrl = "https://fake.api/",
            AverageRatesMainCurrencies = "test/endpoint",
            AverageRatesMinorCurrencies = "test/minor/endpoint",
            AcceptHeader = "text/json"
        });
        var loggerMock = new Mock<ILogger<CurrencyService>>();

        var currencyService = new CurrencyService(_repositoryMock.Object, httpClientFactoryMock.Object, fakeOptions, loggerMock.Object);

        // Act
        var result = await currencyService.GetCurrencyRatesAsync(CurrencyTable.MainCurrencyTable);

        // Assert
        Assert.NotEmpty(result);

        // Sprawdzenie, czy wynik zawiera dokładnie jeden element
        Assert.Single(result);

        // Sprawdzenie, czy zawarty jest odpowiedni kurs waluty
        var rate = result.First();
        Assert.Equal("USD", rate.Currency);
        Assert.Equal("USD", rate.Code);
        Assert.Equal(4.2m, rate.Mid);

    }
}