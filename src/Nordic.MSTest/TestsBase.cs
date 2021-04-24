using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeleScope.Logging;
using TeleScope.Logging.Extensions.Serilog;

namespace Nordic.MSTest
{
	public abstract class TestsBase
	{
		// -- fields

		protected ILogger _log;
		protected const string APP_LOCATION = "App_Data";

		// -- basic methods


		[TestInitialize]
		public virtual void Arrange()
		{
			LoggingProvider.Initialize(
				 new LoggerFactory()
					 .UseTemplate("{Timestamp: HH:mm:ss} [{Level} | {SourceContext:l}] - {Message}{NewLine}{Exception}")
					 .UseLevel(LogLevel.Trace)
					 .AddSerilogConsole());
			_log = LoggingProvider.CreateLogger<TestsBase>();
		}

		[TestCleanup]
		public virtual void Cleanup()
		{

		}
	}
}
