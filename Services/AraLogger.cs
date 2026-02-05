using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ARA.Services
{
	public class AraLogger : ILogger
	{
		private static readonly Lock _lock = new();
		private static readonly AsyncLocal<Stack<object>> _currentScopes = new();
		private readonly LogLevel _minimalLogLevel = LogLevel.Trace;

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		{
			_currentScopes.Value ??= new Stack<object>();
			_currentScopes.Value.Push(state);
			return new LogScope();
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return logLevel >= _minimalLogLevel;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			string message;
			if (formatter != null)
			{
				message = formatter(state, exception);
			}
			else
			{
				message = state?.ToString() ?? string.Empty;
			}

			var logBuilder = new StringBuilder();
			logBuilder.Append($"[{logLevel}] ".PadRight(16));
			var scopes = _currentScopes.Value;
			if (scopes?.Count > 0)
			{
				var scopePath = string.Join(" => ", scopes.Reverse());
				logBuilder.Append($"{scopePath} | ");
			}
			logBuilder.Append($"DateTime: {DateTime.Now:yyyy-MM-dd HH:mm:ss} | ");
			logBuilder.Append($"Message: {message} ");

			lock (_lock)
			{
				File.AppendAllText(Constants.LogFilePath, logBuilder.ToString() + Environment.NewLine);
			}
		}

		private class LogScope : IDisposable
		{
			public void Dispose()
			{
				if (_currentScopes.Value?.Count > 0)
				{
					_currentScopes.Value.Pop();
				}
			}
		}
	}
}
