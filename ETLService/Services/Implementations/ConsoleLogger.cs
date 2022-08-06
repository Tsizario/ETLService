using ETLService.Services.Abstraction;

namespace ETLService.Services.Implementations;

public class ConsoleLogger: ILogger
{
    public void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
        Console.WriteLine(message);
    }

    public void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine();
        Console.WriteLine(message);
    }
}