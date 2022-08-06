using ETLService.Models.InputModels;
using ETLService.Services.Abstraction;
using System.Linq;

namespace ETLService.Services.Implementations;

public class TxtParser: Parser
{
    private readonly ILogger _log;

    public TxtParser(ILogger log)
    {
        _log = log;
    }

    public override string FileExtention => ".txt";

    public override List<Transaction> ParseTransactions(
        string path,
        out int parsedLines,
        out int errors)
    {
        using var sr = new StreamReader(path);
        var fileData = sr.ReadToEnd();

        _log.Info($"File read: {path} | Time: {DateTime.Now.ToLongTimeString()}");

        return ParseFileData(fileData, out parsedLines, out errors);
    }
}