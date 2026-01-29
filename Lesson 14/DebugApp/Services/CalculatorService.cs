namespace DebugApp.Services;

public class CalculatorService
{
    public int Add(int a, int b) => a + b;

    public int Divide(int a, int b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("b 0 bo'lishi mumkin emas.");
        }

        return a / b;
    }
}
