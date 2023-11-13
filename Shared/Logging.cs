namespace Shared
{
	public class Logging
	{
		public static string logfolder = "log";
		public static string logfile = $"{logfolder}\\log.txt";
		public enum LogType
		{
			Info,
			Debug,
			Warning,
			Error,
		}
		public static void PrepareLog()
		{
			if (!Directory.Exists(logfolder)) Directory.CreateDirectory(logfolder);
			if (!File.Exists(logfile)) File.Create(logfile);
			File.WriteAllText(logfile, $"Initialised at {DateTime.Now}\n\n");
		}
		public static void Log(LogType logType, string message)
		{
			try
			{
				ConsoleColor messageColor = ConsoleColor.White;
				if (logType == LogType.Info) messageColor = ConsoleColor.Green;
				if (logType == LogType.Debug) messageColor = ConsoleColor.Magenta;
				if (logType == LogType.Warning) messageColor = ConsoleColor.Yellow;
				if (logType == LogType.Error) messageColor = ConsoleColor.Red;
				Console.Write($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} [");
				Console.ForegroundColor = messageColor;
				Console.Write(logType);
				Console.ResetColor();
				Console.WriteLine($"\t] {message}");
				File.AppendAllText(logfile, $"\n{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} [{logType}] {message}");
			}
			catch (Exception e)
			{
				Console.WriteLine($"ERROR WHILE ERRORLOG>>> {e}");
			}
		}
	}
}
