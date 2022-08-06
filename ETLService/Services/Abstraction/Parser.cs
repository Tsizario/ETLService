using ETLService.Models.InputModels;

namespace ETLService.Services.Abstraction;

public abstract class Parser
{
    public abstract string FileExtention { get; }

    public abstract List<Transaction> ParseTransactions(
        string path,
        out int parsedLines,
        out int errors);

    /// <summary>
    /// data format: each object on separate line, each property separated with ',' by default, order like in class"
    /// </summary>
    protected virtual List<Transaction> ParseFileData(
        string data,
        out int parsedLines,
        out int errors,
        string separator = ",")
    {
        var parsedLinesSum = 0;
        var errorsSum = 0;
        var transactions = new List<Transaction>();

        var validData = data.Split("\n").Where(x => !string.IsNullOrEmpty(x));

        Parallel.ForEach(validData, x =>
        {
            try
            {
                var addressStart = x.IndexOf("“");
                var addressLength = x.LastIndexOf("”") - addressStart - 1;
                var address = x.Substring(addressStart + 1, addressLength);

                x = x.Replace(address, string.Empty);
                var array = x.Split(separator).Select(x => x.Trim()).ToArray();

                var transaction = new Transaction()
                {
                    FirstName = array[0],
                    LastName = array[1],
                    Address = array[2],
                    Payment = Convert.ToDecimal(array[3]),
                    Date = DateTime.ParseExact(array[4], "yyyy-dd-MM", null),
                    AccountNumber = Convert.ToInt64(array[5]),
                    Service = array[6]
                };

                transactions.Add(transaction);
                parsedLinesSum++;
            }
            catch
            {
                errorsSum++;
            }
        });

        if (transactions.Any())
        {
            parsedLines = parsedLinesSum;
            errors = errorsSum;

            return transactions;
        }

        parsedLines = 0;
        errors = 0;

        return null;
    }
}