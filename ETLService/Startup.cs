using ETLService.Models;
using ETLService.Services.Abstraction;
using Timer = System.Timers.Timer;

namespace ETLService;

public class Startup
{
    private readonly IEtlService _etlService;
	private readonly ILogger _log;

	public Startup(
		IEtlService etlService,
		ILogger log)
	{
		_etlService = etlService;
		_log= log;
	}

	private static Timer _timer;
	private static bool stopState = false;

	public void Run()
    {
		AppDomain.CurrentDomain.UnhandledException += ErrorHandler;

		if (string.IsNullOrEmpty(Config.InputPath)
			|| string.IsNullOrEmpty(Config.OutputPath))
		{
			_log.Error("Cannot start app");
			return;
		}

		start:

		var watcher = new FileSystemWatcher(Config.InputPath);
		watcher.EnableRaisingEvents = true;
		watcher.Created += OnFileCreated;

		_timer = new Timer { Interval = GetTimeToSleep() };
		_timer.Elapsed += (_, _) =>
		{
			OnDayChanged();
			_timer.Interval = GetTimeToSleep();
		};
		_timer.Start();

		_log.Info("App started");
		_log.Info("To pause press 'P'");
		_log.Info("To resume press 'P' again");
		_log.Info("To reset press 'R'");
		_log.Info("To stop press 'S'");

		while (true)
		{
			var key = Console.ReadKey();

			switch (key.Key)
			{
				case ConsoleKey.P:
					stopState = !stopState;
					break;
				case ConsoleKey.R: goto start;
				case ConsoleKey.S: return;
			}
		}
	}

	private void OnFileCreated(object sender, FileSystemEventArgs e)
	{
		if (!stopState)
			Task.Run(() => _etlService.OnAddNewFile(e.FullPath));
	}

	private static double GetTimeToSleep()
	{
		var midnightTonight = DateTime.Today.AddDays(1);
		var differenceInMilliseconds = (midnightTonight - DateTime.Now).TotalMilliseconds;
		return differenceInMilliseconds;
	}

	private void OnDayChanged()
	{
		if (!stopState)
			_etlService.MidnightReport();
	}

	private static void OnSystemTimeChanged(object sender, EventArgs e)
	{
		_timer.Interval = GetTimeToSleep();
	}

	private void ErrorHandler(object sender, UnhandledExceptionEventArgs e)
	{
		_log.Error(e.ExceptionObject.ToString());
	}
}