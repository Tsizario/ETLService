namespace HT_1.Services.Abstraction;

public interface ILogger
{
    void Info(string message);

    void Error(string message);
}