using ETLService.Models.InputModels;
using ETLService.Services.Abstraction;

namespace ETLService.Services.Implementations;

public class CsvParser : Parser
{
    private readonly ILogger _log;

    public CsvParser(ILogger log)
    {
        _log = log;
    }

    public override string FileExtention => ".csv";

    public override List<Transaction> ParseTransactions(
        string path,
        out int parsedLines,
        out int errors)
    {
        using var sr = new StreamReader(path);
        var fileData = sr.ReadToEnd();

        _log.Info($"File read: {path} | Time: {DateTime.Now.ToLongTimeString()}");

        return ParseFileData(fileData, out parsedLines, out errors, separator: ";");
    }
}