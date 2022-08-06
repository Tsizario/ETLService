namespace ETLService.Services.Abstraction;

public interface ILogger
{
    void Info(string message);

    void Error(string message);
}