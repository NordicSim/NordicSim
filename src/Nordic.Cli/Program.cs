using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nordic.Abstractions.Data;
using Nordic.Abstractions.Exceptions;
using Nordic.Abstractions.Simulation;
using Nordic.Cli.Options;
using Nordic.Runtime;
using Nordic.Simulation.AdaptedFriis;
using Nordic.Simulation.Battery;
using Nordic.Simulation.Networking;
using Nordic.Simulation.Networking.Devices;
using TeleScope.Logging;
using TeleScope.Logging.Extensions;
using TeleScope.Logging.Extensions.Serilog;
using TeleScope.UI.Cli.Options;

namespace Nordic.Cli
{
	public static class Program
	{
		// -- fields

		private static ILogger _log;

		// -- main

		public static async Task Main(string[] args)
		{
			var options = new CliOptionParser<CliOptions> { Prefix = "--" }.ReadArguments(args);
			LoggingProvider.Initialize(
				 new LoggerFactory()
					 .UseLevel(options.GetLogLevel())
					 .AddSerilogConsole());
			_log = LoggingProvider.CreateLogger(typeof(Program));
			_log.Info($"Starting D3vS1m command line tool...");
			_log.Debug($"Start arguments: {string.Join(' ', args)}");

			try
			{
				var runtime = InitializeRuntime();

				if (!runtime.Validate())
				{
					throw new RuntimeException("The runtime validation failed.");
				}

				await runtime.RunAsync(5);

			}
			catch (Exception ex)
			{
				_log.Critical(ex);
			}
			finally
			{
				WaitAndExit(options.Break);
				_log.Info("Goodby!");
			}
		}

		// -- methods

		private static void WaitAndExit(bool wait = false)
		{
			if (wait)
			{
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}
		}

		private static Runner InitializeRuntime()
		{
			var runner = new Runner();
			runner.Started += (o, e) =>
			{
				_log.Trace($"Runner started.");
			};
			runner.IterationPassed += (o, e) =>
			{
				_log.Trace($"Runner passed iteration");
			};
			runner.Stopped += (o, e) =>
			{
				_log.Trace($"Runner stopped.");
			};

			// radio channel
			var radioSim = new AdaptedFriisSimulator();
			radioSim.With((args) =>
			{
				var radioArgs = args as AdaptedFriisArgs;
				radioArgs.RadioBox.Resolution = 0.2F;
			});

			// energy
			var batterySim = new BatteryPackSimulator();
			batterySim.With((args) =>
			{
				var batteryArgs = args as BatteryArgs;
				var battery = batteryArgs.AddBattery();
				batteryArgs.UpdateDischargeCurrent(battery.Uid, 100);
			});

			// network
			var networkSim = new MeshNetworkSimulator();
			BuildMeshNetwork(networkSim.Arguments as MeshNetworkArgs);

			// pack all simulators in a reop
			var simRepo = new SimulatorRepository();
			simRepo.AddRange(new ISimulatable[] {
				radioSim,
				networkSim,
				batterySim
			});

			runner.BindSimulators(simRepo);

			return runner;
		}

		private static void BuildMeshNetwork(MeshNetworkArgs networkArgs, int devices = 10)
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