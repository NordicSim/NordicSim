using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nordic.Abstractions.Data;
using Nordic.Simulation.MeshNetwork;
using Nordic.Simulation.MeshNetwork.Devices;
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
					 .UseTemplate("{Timestamp: HH:mm:ss} {Level} - {SourceContext:l} >>> {Message}{NewLine}{Exception}")
					 .UseLevel(LogLevel.Trace)
					 .AddSerilogConsole());
			_log = LoggingProvider.CreateLogger<TestsBase>();
		}

		[TestCleanup]
		public virtual void Cleanup()
		{

		}

		// helper methods

		public void BuildMeshNetwork(MeshNetworkArgs networkArgs, int devices = 10)
		{
			var rand = new Random();
			var randomDevices = Enumerable.Range(0, devices)
				.Select(i => new SimpleDevice
				{
					Name = $"dev_{i}",
					Position = new Vertex(rand.Next(0, 100), rand.Next(0, 100), rand.Next(0, 100))
				})
				.ToArray();

			networkArgs.Network.AddRange(randomDevices);
		}
	}
}
