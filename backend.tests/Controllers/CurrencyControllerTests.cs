using Microsoft.AspNetCore.Mvc;
using Moq;
using backend.Controllers;
using backend.Services;

public class CurrencyControllerTests
{
    /// <summary>
    /// Metoda nie odnalazła danych
    /// </summary>
    /// <returns>Zwracany jest kod błędu 404</returns>
    [Fact]
    public async Task GetCurrencyRates_ReturnsNotFoundWhenNoRates()
    {
        // Arrange
        var mockService = new Mock<ICurrencyService>();
        mockService.Setup(service => service.GetCurrencyRatesAsync())
                   .ReturnsAsync([]); // Zwraca puste dane
        var controller = new CurrencyController(mockService.Object);

        // Act
        var result = await controller.GetCurrencyRates();

        // Assert
        var statusCodeResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, statusCodeResult.StatusCode);

    }

    /// <summary>
    /// Następuje błąd serwera podczas próby pobrania danych
    /// </summary>
    /// <returns>Wyświetlany jest błąd serwera - 500</returns>
    [Fact]
    public async Task GetCurrencyRates_ReturnsInternalServerErrorWhenExceptionThrown()
    {
        // Arrange
        var mockService = new Mock<ICurrencyService>();
        mockService.Setup(service => service.GetCurrencyRatesAsync())
                   .ThrowsAsync(new Exception("Testowy błąd serwera"));
        var controller = new CurrencyController(mockService.Object);

        // Act
        var result = await controller.GetCurrencyRates();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
}