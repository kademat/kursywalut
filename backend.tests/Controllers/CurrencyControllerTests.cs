using Microsoft.AspNetCore.Mvc;
using Moq;
using backend.Controllers;
using backend.Services;

public class CurrencyControllerTests
{
    private Mock<ICurrencyService> _mockService;
    private CurrencyController _controller;

    public CurrencyControllerTests()
    {
        _mockService = new Mock<ICurrencyService>();
        _controller = new CurrencyController(_mockService.Object);
    }

    [Theory]
    [InlineData(CurrencyTable.MainCurrencyTable)]
    [InlineData(CurrencyTable.MinorCurrencyTable)]
    public async Task GetCurrencyRates_ReturnsNotFoundWhenNoRates(CurrencyTable currencyTable)
    {
        // Arrange
        _mockService.Setup(service => service.GetCurrencyRatesAsync(currencyTable))
                    .ReturnsAsync([]); // Puste dane

        // Act
        var result = currencyTable switch
        {
            CurrencyTable.MainCurrencyTable => await _controller.GetMainCurrencyRates(),
            CurrencyTable.MinorCurrencyTable => await _controller.GetMinorCurrencyRates(),
            _ => throw new ArgumentException($"Tabela nie jest wspierana: {currencyTable}")
        };

        // Assert
        var statusCodeResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, statusCodeResult.StatusCode);
    }

    [Theory]
    [InlineData(CurrencyTable.MainCurrencyTable)]
    [InlineData(CurrencyTable.MinorCurrencyTable)]
    public async Task GetCurrencyRates_ReturnsInternalServerErrorWhenExceptionThrown(CurrencyTable currencyTable)
    {
        // Arrange
        _mockService.Setup(service => service.GetCurrencyRatesAsync(currencyTable))
                    .ThrowsAsync(new Exception("Testowy błąd serwera"));

        // Act
        var result = currencyTable switch
        {
            CurrencyTable.MainCurrencyTable => await _controller.GetMainCurrencyRates(),
            CurrencyTable.MinorCurrencyTable => await _controller.GetMinorCurrencyRates(),
            _ => throw new ArgumentException($"Tabela nie jest wspierana: {currencyTable}")
        };

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}