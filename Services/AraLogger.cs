using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ARA.Services
{
	public class AraLogger : ILogger
	{
		private readonly AsyncLocal<Stack<object>> _currentScopes = new();
		private readonly LogLevel _minimalLogLevel = LogLevel.Trace;

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		{
			_currentScopes.Value ??= new Stack<object>();
			_currentScopes.Value.Push(state);
			return new LogScope(this);
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
			Debug.WriteLine(message);
		}

		private class LogScope(AraLogger logger) : IDisposable
		{
			private readonly AraLogger _logger = logger;

			public void Dispose()
			{
				if (_logger._currentScopes.Value?.Count > 0)
				{
					_logger._currentScopes.Value.Pop();
				}
			}
		}
	}
}
