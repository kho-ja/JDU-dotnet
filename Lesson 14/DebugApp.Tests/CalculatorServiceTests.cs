using DebugApp.Services;

namespace DebugApp.Tests;

public class CalculatorServiceTests
{
    [Fact]
    public void Add_ReturnsSum()
    {
        var service = new CalculatorService();
        var result = service.Add(2, 3);
        Assert.Equal(5, result);
    }

    [Fact]
    public void Divide_ByNonZero_ReturnsQuotient()
    {
        var service = new CalculatorService();
        var result = service.Divide(10, 2);
        Assert.Equal(5, result);
    }

    [Fact]
    public void Divide_ByZero_Throws()
    {
        var service = new CalculatorService();
        Assert.Throws<DivideByZeroException>(() => service.Divide(10, 0));
    }
}
