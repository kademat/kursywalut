using backend.Models;
using backend.Repositories;
using backend.Services;
using Moq;

public class CurrencyServiceTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly CurrencyService _currencyService;

    public CurrencyServiceTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _currencyService = new CurrencyService(_repositoryMock.Object, _httpClientFactoryMock.Object);
    }

    [Fact]
    public async Task GetTodayRatesFromRepositoryAsync_ReturnsRatesForToday()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now).ToString();
        var expectedRates = new List<NbpRate>
        {
            new() { Currency = "USD", Code = "USD", Mid = 4.5m },
            new() { Currency = "EUR", Code = "EUR", Mid = 4.8m }
        };
        var nbpTables = new List<NbpTable>
        {
            new() { EffectiveDate = today, Rates = expectedRates }
        };

        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(nbpTables);

        // Act
        var result = await _currencyService.GetTodayRatesFromRepositoryAsync();

        // Assert
        Assert.Equal(expectedRates, result);
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetTodayRatesFromRepositoryAsync_ReturnsEmptyListIfNoRatesForToday()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<NbpTable>());

        // Act
        var result = await _currencyService.GetTodayRatesFromRepositoryAsync();

        // Assert
        Assert.Empty(result);
        _repositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}