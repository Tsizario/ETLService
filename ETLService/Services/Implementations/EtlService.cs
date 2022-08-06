using ETLService.Models;
using ETLService.Models.OutputModels;
using ETLService.Services.Abstraction;
using ETLService.Services.Extensions;
using ETLService.Services.Templates;
using HT_1.Services.Extentions;
using Newtonsoft.Json;

namespace ETLService.Services.Implementations;

public class EtlService: IEtlService
{
	private Dictionary<string, Lazy<Parser>> _parses { get; }
	private ILogger _log { get; }

	public EtlService(IEnumerable<Parser> parses, ILogger log)
	{
		_parses = parses.Select(x => new { Key = x.FileExtention, Value = new Lazy<Parser>(() =>x) })
			.ToDictionary(x => x.Key, x => x.Value);
		_log = log;

		ClearData();
	}

	private int _parsedFiles { get; set; }
	private int _parsedLines { get; set; }
	private int _foundErrors { get; set; }
	private List<string> _invalidFiles { get; set; } = new();
	private string TodatDateFormat => "MM-dd-yyyy";

	public Task OnAddNewFile(string path)
	{
		if (!_parses.ContainsKey(path.Substring(path.LastIndexOf('.'))))
			return Task.CompletedTask;

		_log.Info($"Start: {path} | Time: {DateTime.Now.ToLongTimeString()}");

		var info = ParseFile(path);
		if (info != null)
			CreateReport(info);

		_log.Info($"End: {path} | Time: {DateTime.Now.ToLongTimeString()}");

		return Task.CompletedTask;
	}

	public void MidnightReport()
	{
		GenerateMetaLog(@$"{Config.OutputPath}\{DateTime.Now.ToString(TodatDateFormat)}");
		ClearData();
	}

	#region Helpers

	private TransactionInfo[] ParseFile(string path)
	{
		var parser = _parses.FirstOrDefault(x => path.EndsWith(x.Key)).Value;

		if (parser == null)
			throw new Exception("Unexpected file extantion");

		_log.Info($"Parser found: {path} | Time: {DateTime.Now.ToLongTimeString()}");

		var transactions = parser.Value.ParseTransactions(
			path,
			out var parsedLines,
			out var errors
		);

		_log.Info($"Parsed: {path} | Time: {DateTime.Now.ToLongTimeString()}");

		if (transactions == null)
		{
			_invalidFiles.Add(path);
			return null;
		}

		_parsedFiles++;
		_parsedLines += parsedLines;
		_foundErrors += errors;

		var infos = transactions
				.GroupBy(x => x.Address.ToAddress().City)
				.Select(x => new TransactionInfo()
				{
					City = x.Key,
					Services = x.GroupBy(y => y.Service)
						.Select(y => new Service
						{
							Name = y.Key,
							Payers = y.Select(z => new Payer()
							{
								FirstName = z.FirstName,
								LastName = z.LastName,
								Payment = z.Payment,
								AccountNumber = z.AccountNumber
							}).ToArray()
						}).ToArray()
				}).ToArray();

		_log.Info($"Grouped: {path} | Time: {DateTime.Now.ToLongTimeString()}");

		return infos;
	}

	private void CreateReport(TransactionInfo[] info)
	{
		var directoryPath = @$"{Config.OutputPath}\{DateTime.Now.ToString(TodatDateFormat)}";
		var isExist = Directory.Exists(directoryPath);
		if (!isExist)
			Directory.CreateDirectory(directoryPath);

		using var sw = new StreamWriter($@"{directoryPath}\output{_parsedFiles}.json");
		sw.WriteLine(JsonConvert.SerializeObject(info, Formatting.Indented));
	}

	private void GenerateMetaLog(string path)
	{
		var log = string.Format(
			OutputTemplates.MetaLogTemplate,
			_parsedFiles,
			_parsedLines,
			_foundErrors,
			_invalidFiles.JoinString(", ")
		);

		var directoryPath = @$"{Config.OutputPath}\{DateTime.Now.ToString(TodatDateFormat)}";
		var isExist = Directory.Exists(directoryPath);
		if (!isExist)
			Directory.CreateDirectory(directoryPath);

		using var fs = File.CreateText(@$"{path}\meta.log");
		fs.WriteLine(log);
	}

	private void ClearData()
	{
		_parsedFiles = default;
		_parsedLines = default;
		_foundErrors = default;
		_invalidFiles = new();
	}

	#endregion
}